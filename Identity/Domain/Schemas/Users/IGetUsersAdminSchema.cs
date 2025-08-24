using Base.Domain.Schemas.Interfaces.Requests;

namespace Domain.Schemas.Users;

public interface IGetUsersAdminSchema : IFullWithSearchPaginatedRequest
{
    string? Email { get; init; }
    string? UserName { get; init; }
    
    DateTime? CreatedFrom { get; init; }
    DateTime? CreatedTo { get; init; }
}