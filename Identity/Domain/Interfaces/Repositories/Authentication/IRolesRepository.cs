using Base.Domain.Infrastructure.Interfaces.Repositories;
using Base.Domain.Schemas;
using Base.Domain.Schemas.Interfaces.Requests;
using Domain.Models.Authentication;

namespace Domain.Interfaces.Repositories.Authentication;

public interface IRolesRepository : IBaseRepository<RoleModel, string>
{
    Task<PaginatedResponse<RoleModel>> GetAsync(IPaginatedRequest schema, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name);
}