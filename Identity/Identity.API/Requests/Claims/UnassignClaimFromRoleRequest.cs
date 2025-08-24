using Base.API.Requests;

namespace Identity.API.Requests.Claims;

public class UnassignClaimFromRoleRequest : BaseAuthRequest
{
    public required string RoleId { get; init; }
}