using Base.API.Requests;

namespace Identity.API.Requests.Claims;

public class AssignClaimToRoleRequest : BaseAuthRequest
{
    public required string RoleId { get; init; }
}