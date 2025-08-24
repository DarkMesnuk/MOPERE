using Base.API.Requests;

namespace Identity.API.Requests.Authentication.Admin;

public class BlockingUserRequest : BaseAuthRequest
{
    public required string UserId { get; init; }
    public required bool IsBlocked { get; init; }
}