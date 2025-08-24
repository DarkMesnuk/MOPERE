using System.Security.Claims;

namespace Identity.Extensions;

public static class AuthExtensions
{
    public static string? GetActiveUserId(this ClaimsPrincipal user)
    {
        var userClaim = user.Claims.FirstOrDefault(claim => claim.Type == "Id");

        return userClaim?.Value;
    }
}