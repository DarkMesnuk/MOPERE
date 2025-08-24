using Base.API.Requests;

namespace Identity.API.Requests.Authentication;

public class ChangePasswordRequest : BaseAuthRequest
{
    public required string NewPassword { get; init; }
    public required string OldPassword { get; init; }
}