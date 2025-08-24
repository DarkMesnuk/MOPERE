using Base.API.Requests;

namespace Identity.API.Requests.Users.Admin;

public class CreateAdminUserRequest : BaseAuthRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    
    public required string RoleId { get; init; }
}