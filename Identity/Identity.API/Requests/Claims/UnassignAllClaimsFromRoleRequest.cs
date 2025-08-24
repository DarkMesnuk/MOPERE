using Base.API.Requests;

namespace Identity.API.Requests.Claims;

public class UnassignAllClaimsFromRoleRequest : BaseAuthRequest
{
    public required string RoleId { get; init; }
}