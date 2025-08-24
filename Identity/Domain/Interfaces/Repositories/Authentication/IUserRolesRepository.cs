using Base.Domain.Infrastructure.Interfaces.Repositories;
using Domain.Models.Users;

namespace Domain.Interfaces.Repositories.Authentication;

public interface IUserRolesRepository : IBaseRepository<UserRoleModel, string>
{
    Task<bool> ExistsAsync(string userId, string roleId, CancellationToken cancellationToken = default);
    Task<List<UserRoleModel>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string userId, string roleId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string userId, IEnumerable<string> roleIds, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(IEnumerable<string> userIds, string roleId, CancellationToken cancellationToken = default);
    Task<bool> DeleteByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteByUserAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default);
}