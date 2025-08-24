using System.ComponentModel.DataAnnotations;
using Base.API.Requests;

namespace Identity.API.Requests.Authentication;

public class ConfirmChangeEmailRequest : BaseAuthRequest
{
    [EmailAddress]
    public required string NewEmail { get; init; }
    public required string Code { get; init; }
}