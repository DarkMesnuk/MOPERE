namespace Base.Domain.Schemas.Interfaces.Responses;

public interface IPaginatedResponse<T>
{
    public long Count { get; set; }
    public long TotalCount { get; set; }
    public IEnumerable<T> Dtos { get; set; }
}