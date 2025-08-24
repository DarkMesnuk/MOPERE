using Base.API.Requests;

namespace Identity.API.Requests.Claims;

public class IsUserHaveClaimRequest : BaseAuthRequest
{
    public required string UserId { get; init; }
    public required string ClaimId { get; init; }
}