namespace Base.Domain.Schemas;

public class UpdateOrSetNullFieldSchema<T>
{
    public T? Value { get; init; }
    public bool? SetNull { get; init; }
}