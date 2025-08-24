using AutoMapper;
using Base.Infrastructure.Database.PostgreSQL.Repositories;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Models.Users;
using Infrastructure.Database.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database.PostgreSQL.Repositories;

public class UserRolesRepository(
    MopereIdentityContext context, 
    ILogger<UserRolesRepository> logger, 
    IMapper mapper
) : BaseRepository<UserRoleEntity, UserRoleModel, string>(context, logger, mapper), IUserRolesRepository
{
    public Task<bool> ExistsAsync(string userId, string roleId, CancellationToken cancellationToken = default)
    {
        return base.ExistsAsync(x => x.UserId == userId && x.RoleId == roleId, cancellationToken);
    }

    public async Task<List<UserRoleModel>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var userRoles = await GetBase
            .Include(x => x.Role)
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return MapToModel(userRoles);
    }

    public Task<bool> DeleteAsync(string userId, string roleId, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(x => x.UserId == userId && x.RoleId == roleId, cancellationToken);
    }

    public Task<bool> DeleteAsync(string userId, IEnumerable<string> roleIds, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(x => roleIds.Contains(x.UserId) && x.UserId == userId, cancellationToken);
    }

    public Task<bool> DeleteAsync(IEnumerable<string> userIds, string roleId, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(x => userIds.Contains(x.UserId) && x.RoleId == roleId, cancellationToken);
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