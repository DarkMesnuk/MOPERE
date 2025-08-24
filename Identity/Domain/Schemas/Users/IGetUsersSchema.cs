using Base.Domain.Schemas.Interfaces.Requests;

namespace Domain.Schemas.Users;

public interface IGetUsersSchema : IFullWithSearchPaginatedRequest
{
    string? Email { get; init; }
    string? UserName { get; init; }
}