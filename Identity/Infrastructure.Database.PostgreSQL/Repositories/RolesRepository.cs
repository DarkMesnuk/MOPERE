using AutoMapper;
using Base.Domain.Exceptions;
using Base.Domain.Extensions;
using Base.Domain.Schemas;
using Base.Domain.Schemas.Interfaces.Requests;
using Base.Infrastructure.Database.PostgreSQL.Extensions;
using Base.Infrastructure.Database.PostgreSQL.Repositories;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Models.Authentication;
using Infrastructure.Database.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database.PostgreSQL.Repositories;

public class RolesRepository(
    MopereIdentityContext context, 
    ILogger<RolesRepository> logger, 
    IMapper mapper
) : BaseRepository<RoleEntity, RoleModel, string>(context, logger, mapper), IRolesRepository
{
    public override async Task<RoleModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var role = await GetBase
            .Include(x => x.Claims)!
                .ThenInclude(x => x.Claim)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        if (role.IsNullOrDefault())
            throw new NotFoundException(nameof(RoleModel));

        return MapToModel(role!);
    }
    
    public async Task<PaginatedResponse<RoleModel>> GetAsync(IPaginatedRequest schema, CancellationToken cancellationToken = default)
    {
        var query = GetBase
            .Include(x => x.Claims)!
                .ThenInclude(x => x.Claim)
            .AsQueryable();
        
        var roles = await query
            .OrderBy(x => x.Id)
            .Page(schema)
            .ToPaginatedAsync(query, cancellationToken);
        
        return MapToModel(roles);
    }

    public Task<bool> ExistsByNameAsync(string name)
    {
        var normalizedName = name.ToNormalized();
        return ExistsAsync(x => x.NormalizedName == normalizedName);
    }
}