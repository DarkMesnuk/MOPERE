using Base.Domain.Infrastructure.Interfaces.Repositories;
using Domain.Models.Authentication;

namespace Domain.Interfaces.Repositories.Authentication;

public interface IUserClaimsRepository : IBaseRepository<UserClaimModel, int>
{
    Task<bool> ExistsAsync(string userId, string claimId, CancellationToken cancellationToken = default);
    Task<List<UserClaimModel>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string userId, string claimId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string userId, IEnumerable<string> claimIds, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(IEnumerable<string> userIds, string claimId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(IEnumerable<string> userIds, IEnumerable<string> claimIds, CancellationToken cancellationToken = default);
    Task<bool> DeleteByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteByUserAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default);
}