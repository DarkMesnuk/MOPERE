using System.ComponentModel.DataAnnotations;

namespace Identity.API.Requests.Authentication;

public class ConfirmRegistrationRequest
{
    [EmailAddress]
    public required string Email { get; init; }
    public required string Code { get; init; }
}