using Base.API.Requests;

namespace Identity.API.Requests.Claims;

public class SetClaimsToUserRequest : BaseAuthRequest
{
    public required string UserId { get; init; }
    public required ICollection<string> ClaimIds { get; init; }
}