using Base.API.Requests;

namespace Identity.API.Requests.Claims;

public class SetClaimsToRoleRequest : BaseAuthRequest
{
    public required string RoleId { get; init; }
    public required ICollection<string> ClaimIds { get; init; }
}