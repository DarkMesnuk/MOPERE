namespace Base.Domain.Schemas;

public class RenFilterSchema<T> where T : struct
{
    public T? From { get; set; }
    public T? To { get; set; }
    public T? Equal { get; set; }
    public T? NotEqual { get; set; }
}
