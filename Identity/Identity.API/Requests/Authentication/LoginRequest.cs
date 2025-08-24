using System.ComponentModel.DataAnnotations;

namespace Identity.API.Requests.Authentication;

public class LoginRequest
{
    [EmailAddress]
    public required string Email { get; init; }
    public required string Password { get; init; }
}