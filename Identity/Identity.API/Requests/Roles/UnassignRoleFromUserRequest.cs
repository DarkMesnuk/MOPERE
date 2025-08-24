using Base.API.Requests;

namespace Identity.API.Requests.Roles;

public class UnassignRoleFromUserRequest : BaseAuthRequest
{
    public required string UserId { get; init; }
}