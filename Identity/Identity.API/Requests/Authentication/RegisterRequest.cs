using System.ComponentModel.DataAnnotations;

namespace Identity.API.Requests.Authentication;

public class RegisterRequest
{
    [EmailAddress]
    public required string Email { get; init; }
    public required string Password { get; init; }
}