using System.ComponentModel.DataAnnotations;
using Base.API.Requests;

namespace Identity.API.Requests.Authentication;

public class ChangeEmailRequest : BaseAuthRequest
{
    [EmailAddress]
    public required string Email { get; init; }
}