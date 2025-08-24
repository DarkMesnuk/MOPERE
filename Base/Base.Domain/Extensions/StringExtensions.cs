namespace Base.Domain.Extensions;

public static class StringExtensions
{
    public static string ToNormalized(this string value) => value.Trim().ToUpperInvariant();
}