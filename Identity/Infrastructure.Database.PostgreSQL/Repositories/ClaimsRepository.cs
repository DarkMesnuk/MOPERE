using AutoMapper;
using Base.Domain.Extensions;
using Base.Domain.Schemas;
using Base.Infrastructure.Database.PostgreSQL.Repositories;
using Base.Infrastructure.Database.PostgreSQL.Extensions;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Models.Authentication;
using Domain.Schemas.Claims;
using Infrastructure.Database.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database.PostgreSQL.Repositories;

public class ClaimsRepository(
    MopereIdentityContext context,
    ILogger<ClaimsRepository> logger,
    IMapper mapper
) : BaseRepository<ClaimEntity, ClaimModel, string>(context, logger, mapper), IClaimsRepository
{
    public bool Exists(string type, string value)
    {
        var typeNormalized = type.ToNormalized();
        var valueNormalized = value.ToNormalized();
        return Exists(x => x.Type == typeNormalized && x.Value == valueNormalized);
    }

    public async Task<PaginatedResponse<ClaimModel>> GetAsync(IGetClaimsSchema schema, CancellationToken cancellationToken = default)
    {
        var query = GetBase.Include(x => x.Roles).AsQueryable();

        if (!string.IsNullOrWhiteSpace(schema.RoleId))
        {
            query = query.Where(x => x.Roles != null && x.Roles.Any(roleClaimLinkEntity => roleClaimLinkEntity.RoleId == schema.RoleId));
        }

        var claims = await query
            .OrderBy(x => x.Id)
            .Page(schema)
            .ToPaginatedAsync(query, cancellationToken);
        
        return MapToModel(claims);
    }

    public async Task<List<ClaimModel>> GetByRoleIdAsync(string roleId, CancellationToken cancellationToken = default)
    {
        var claims = await GetBase
            .Include(x => x.Roles)
            .Where(x => x.Roles != null && x.Roles.Any(y => y.RoleId == roleId))
            .ToListAsync(cancellationToken);
        
        return MapToModel(claims);
    }

    public async Task<List<ClaimModel>> GetByValueAndTypeAsync(IEnumerable<ClaimModel> claims, CancellationToken cancellationToken = default)
    {
        var pairs = claims
            .Select(c => new { c.Value, c.Type })
            .ToHashSet();
        
        var claimsWithFullInfo = new List<ClaimEntity>();

        foreach (var pair in pairs)
        {
            var claimFullInfos = await GetBase
                .FirstOrDefaultAsync(x => x.Value == pair.Value && x.Type == pair.Type, cancellationToken);

            if (claimFullInfos != null)
            {
                claimsWithFullInfo.Add(claimFullInfos);
            }
        }
        
        return MapToModel(claimsWithFullInfo);
    }
}