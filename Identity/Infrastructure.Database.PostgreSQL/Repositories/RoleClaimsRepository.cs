using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Base.Domain.Exceptions;
using Base.Infrastructure.Database.PostgreSQL.Repositories;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Models.Authentication;
using Infrastructure.Database.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database.PostgreSQL.Repositories;

public class RoleClaimsRepository(
    MopereIdentityContext context, 
    ILogger<RoleClaimsRepository> logger, 
    IMapper mapper
) : BaseRepository<RoleClaimEntity, RoleClaimModel, int>(context, logger, mapper), IRoleClaimsRepository
{
    public new async Task<RoleClaimModel> CreateAsync(RoleClaimModel model, CancellationToken cancellationToken = default)
    {
        var exists = await ExistsAsync(x => x.RoleId == model.Role.Id && x.ClaimId == model.Claim.Id, cancellationToken);

        if (exists)
            throw new AlreadyExistsException("");
        
        try
        {
            var entity = MapToEntity(model);
            var query = DbSet.Add(entity);

            entity = query.Entity;
            
            await context.SaveChangesAsync(cancellationToken);
            
            context.Entry(entity).State = EntityState.Detached;
            
            model = MapToModel(entity);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} create failed", nameof(RoleClaimModel));
            throw new CreationFailedException();
        }

        return model;
    }

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public new async Task<bool> CreateAsync(IEnumerable<RoleClaimModel> models, CancellationToken cancellationToken = default)
    {
        var userIds = models.Select(m => m.Role!.Id).ToList();
        var claimIds = models.Select(m => m.Claim!.Id).ToList();

        var existing = await GetBase
            .Where(uc => userIds.Contains(uc.RoleId) && claimIds.Contains(uc.ClaimId))
            .Select(uc => new { uc.RoleId, uc.ClaimId })
            .ToListAsync(cancellationToken);

        var existingSet = existing
            .Select(e => (e.RoleId, e.ClaimId))
            .ToHashSet();

        var toCreate = models
            .Where(m => !existingSet.Contains((m.Role.Id, m.Claim.Id)))
            .ToList();
        
        try
        {
            var entities = MapToEntity(toCreate).ToList();

            DbSet.AddRange(entities);
            
            await context.SaveChangesAsync(cancellationToken);
            
            foreach (var entity in entities)
                context.Entry(entity).State = EntityState.Detached;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} range creations failed", nameof(RoleClaimModel));
            throw new CreationFailedException();
        }

        return true;
    }

    public async Task<List<RoleClaimModel>> GetByRoleAsync(string roleId, CancellationToken cancellationToken = default)
    {
        var userClaims = await GetBase
            .Include(x => x.Claim)
            .Where(x => x.RoleId == roleId)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return MapToModel(userClaims);
    }

    public Task<bool> DeleteAsync(string roleId, string claimId, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(x => x.RoleId == roleId && x.ClaimId == claimId, cancellationToken);
    }

    public Task<bool> DeleteAsync(string roleId, IEnumerable<string> claimIds, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(x => claimIds.Contains(x.ClaimId) && x.RoleId == roleId, cancellationToken);
    }

    public Task<bool> DeleteByRoleAsync(string roleId, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(x => x.RoleId == roleId, cancellationToken);
    }
}