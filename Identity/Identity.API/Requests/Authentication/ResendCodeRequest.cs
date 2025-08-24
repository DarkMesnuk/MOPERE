using System.ComponentModel.DataAnnotations;

namespace Identity.API.Requests.Authentication;

public class ResendCodeRequest
{
    [EmailAddress]
    public required string Email { get; init; }
}