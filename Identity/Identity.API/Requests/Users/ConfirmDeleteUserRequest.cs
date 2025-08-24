using Base.API.Requests;

namespace Identity.API.Requests.Users;

public class ConfirmDeleteUserRequest : BaseAuthRequest
{
    public required string Code { get; init; }
}