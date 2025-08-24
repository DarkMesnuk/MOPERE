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

public class UserClaimsRepository(
    MopereIdentityContext context, 
    ILogger<UserClaimsRepository> logger, 
    IMapper mapper
) : BaseRepository<UserClaimEntity, UserClaimModel, int>(context, logger, mapper), IUserClaimsRepository
{
    public new async Task<UserClaimModel> CreateAsync(UserClaimModel model, CancellationToken cancellationToken = default)
    {
        var exists = await ExistsAsync(x => x.UserId == model.User.Id && x.ClaimId == model.Claim.Id, cancellationToken);

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
            logger.LogError(exception, "{Name} create failed", nameof(UserClaimModel));
            throw new CreationFailedException();
        }

        return model;
    }

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public new async Task<bool> CreateAsync(IEnumerable<UserClaimModel> models, CancellationToken cancellationToken = default)
    {
        var userIds = models.Select(m => m.User.Id).ToList();
        var claimIds = models.Select(m => m.Claim.Id).ToList();

        var existing = await GetBase
            .Where(uc => userIds.Contains(uc.UserId) && claimIds.Contains(uc.ClaimId))
            .Select(uc => new { uc.UserId, uc.ClaimId })
            .ToListAsync(cancellationToken);

        var existingSet = existing
            .Select(e => (e.UserId, e.ClaimId))
            .ToHashSet();

        var toCreate = models
            .Where(m => !existingSet.Contains((m.User.Id, m.Claim.Id)))
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
            logger.LogError(exception, "{Name} range creations failed", nameof(UserClaimModel));
            throw new CreationFailedException();
        }

        return true;
    }

    public Task<bool> ExistsAsync(string userId, string claimId, CancellationToken cancellationToken = default)
    {
        return base.ExistsAsync(x => x.UserId == userId && x.ClaimId == claimId, cancellationToken);
    }

    public async Task<List<UserClaimModel>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var userClaims = await GetBase
            .Include(x => x.Claim)
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return MapToModel(userClaims);
    }

    public Task<bool> DeleteAsync(string userId, string claimId, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(x => x.UserId == userId && x.ClaimId == claimId, cancellationToken);
    }

    public Task<bool> DeleteAsync(string userId, IEnumerable<string> claimIds, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(x => claimIds.Contains(x.ClaimId) && x.UserId == userId, cancellationToken);
    }

    public Task<bool> DeleteAsync(IEnumerable<string> userIds, string claimId, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(x => userIds.Contains(x.UserId) && x.ClaimId == claimId, cancellationToken);
    }

    public Task<bool> DeleteAsync(IEnumerable<string> userIds, IEnumerable<string> claimIds, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(x => claimIds.Contains(x.ClaimId) && userIds.Contains(x.UserId), cancellationToken);
    }

    public Task<bool> DeleteByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(x => x.UserId == userId, cancellationToken);
    }

    public Task<bool> DeleteByUserAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(x => userIds.Contains(x.UserId), cancellationToken);
    }
}