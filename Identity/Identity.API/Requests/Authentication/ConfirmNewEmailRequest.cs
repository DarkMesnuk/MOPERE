using Base.API.Requests;

namespace Identity.API.Requests.Authentication;

public class ConfirmNewEmailRequest : BaseAuthRequest
{
    public required string Code { get; init; }
}