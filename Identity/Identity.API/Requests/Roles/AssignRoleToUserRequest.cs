using Base.API.Requests;

namespace Identity.API.Requests.Roles;

public class AssignRoleToUserRequest : BaseAuthRequest
{
    public required string UserId { get; init; }
}