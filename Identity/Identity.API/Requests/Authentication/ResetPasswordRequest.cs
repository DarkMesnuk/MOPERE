using System.ComponentModel.DataAnnotations;

namespace Identity.API.Requests.Authentication;

public class ResetPasswordRequest
{
    [EmailAddress]
    public required string Email { get; init; }
    public required string NewPassword { get; init; }
    public required string Code { get; init; }
}