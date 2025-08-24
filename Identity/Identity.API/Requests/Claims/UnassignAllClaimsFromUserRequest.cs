using Base.API.Requests;

namespace Identity.API.Requests.Claims;

public class UnassignAllClaimsFromUserRequest : BaseAuthRequest
{
    public required string UserId { get; init; }
}