using System.Security.Claims;
using Domain.Models.Authentication;

namespace Identity.Extensions;

public static class ClaimExtensions
{
    public static Claim ToClaim(this ClaimModel claimModel)
    {
        return new Claim(claimModel.Type, claimModel.Value);
    }
}