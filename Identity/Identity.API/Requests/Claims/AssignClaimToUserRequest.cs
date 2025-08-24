using Base.API.Requests;

namespace Identity.API.Requests.Claims;

public class AssignClaimToUserRequest : BaseAuthRequest
{
    public required string UserId { get; init; }
}