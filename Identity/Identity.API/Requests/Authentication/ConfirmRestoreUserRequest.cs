namespace Identity.API.Requests.Authentication;

public class ConfirmRestoreUserRequest
{
    public required string Email { get; init; }    
    public required string Code { get; init; }
}