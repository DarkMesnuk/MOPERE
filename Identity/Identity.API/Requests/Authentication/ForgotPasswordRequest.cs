using System.ComponentModel.DataAnnotations;

namespace Identity.API.Requests.Authentication;

public class ForgotPasswordRequest
{
    [EmailAddress]
    public required string Email { get; init; }
}