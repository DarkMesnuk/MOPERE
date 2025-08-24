using Base.Domain.Chekers;
using Identity.Interfaces;

namespace Service;

public class IdentityAccessChecker(
    IIdentityService identityService
) : IIdentityAccessChecker
{
    public Task ThrowIfUserNotPossibleToAuthAsync(string? userId)
    {
        return identityService.ThrowIfUserNotPossibleToAuthAsync(userId);
    }
}