namespace Base.Domain.Chekers;

public interface IIdentityAccessChecker
{
    Task ThrowIfUserNotPossibleToAuthAsync(string? userId);
}