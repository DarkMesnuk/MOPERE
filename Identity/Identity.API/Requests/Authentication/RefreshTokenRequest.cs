namespace Identity.API.Requests.Authentication;

public class RefreshTokenRequest
{
    public required string OldToken { get; init; }
    public required string RefreshToken { get; init; }
}