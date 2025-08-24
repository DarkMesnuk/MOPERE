using Base.Domain.Infrastructure.Interfaces.Repositories;
using Base.Domain.Schemas;
using Domain.Models.Users;
using Domain.Schemas.Users;

namespace Domain.Interfaces.Repositories.Users;

public interface IUsersRepository : IBaseRepository<UserModel, string>
{
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByUserNameAsync(string userName);
    Task<UserModel> GetByEmailAsync(string email);
    Task<PaginatedResponse<UserModel>> GetAsync(IGetUsersAdminSchema schema, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<UserModel>> GetAsync(IGetUsersSchema schema, CancellationToken cancellationToken = default);
    Task<UserModel> GetWithIncludesByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<List<string>> GetIdsByRoleAsync(string roleId, CancellationToken cancellationToken = default);
    Task UpdateBySchemaAsync(UserModel user, IUpdateUserSchema schema);
}