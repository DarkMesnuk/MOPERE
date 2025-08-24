using Base.API.Requests;

namespace Identity.API.Requests.Claims;

public class UnassignClaimFromUserRequest : BaseAuthRequest
{
    public required string UserId { get; init; }
}