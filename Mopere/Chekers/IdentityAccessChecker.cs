using Base.Domain.Chekers;

namespace Mopere.Chekers;

public class IdentityAccessChecker : IIdentityAccessChecker
{
    public Task ThrowIfUserNotPossibleToAuthAsync(string? userId)
    {
        throw new NotImplementedException();
    }
}