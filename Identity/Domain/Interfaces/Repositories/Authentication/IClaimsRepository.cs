using Base.Domain.Infrastructure.Interfaces.Repositories;
using Base.Domain.Schemas;
using Domain.Models.Authentication;
using Domain.Schemas.Claims;

namespace Domain.Interfaces.Repositories.Authentication;

public interface IClaimsRepository : IBaseRepository<ClaimModel, string>
{
    bool Exists(string type, string value);
    Task<PaginatedResponse<ClaimModel>> GetAsync(IGetClaimsSchema schema, CancellationToken cancellationToken = default);
    Task<List<ClaimModel>> GetByRoleIdAsync(string roleId, CancellationToken cancellationToken = default);
    Task<List<ClaimModel>> GetByValueAndTypeAsync(IEnumerable<ClaimModel> claims, CancellationToken cancellationToken = default);
}