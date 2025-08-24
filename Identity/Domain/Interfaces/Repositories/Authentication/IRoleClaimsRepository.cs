using Base.Domain.Infrastructure.Interfaces.Repositories;
using Domain.Models.Authentication;

namespace Domain.Interfaces.Repositories.Authentication;

public interface IRoleClaimsRepository : IBaseRepository<RoleClaimModel, int>
{
    Task<List<RoleClaimModel>> GetByRoleAsync(string roleId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string roleId, string claimId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string roleId, IEnumerable<string> claimIds, CancellationToken cancellationToken = default);
    Task<bool> DeleteByRoleAsync(string roleId, CancellationToken cancellationToken = default);
}