using Base.API.Requests;

namespace Identity.API.Requests.Roles;

public class UnassignAllRolesFromUserRequest : BaseAuthRequest
{
    public required string UserId { get; init; }
}