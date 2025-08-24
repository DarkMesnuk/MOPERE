using Base.Domain.Schemas;

namespace Base.Domain.Extensions;

public static class SchemasExtensions
{
    public static bool HasValue<T>(this UpdateOrSetNullFieldSchema<T>? schema)
    {
        return schema != null && schema.Value != null;
    }
}