using Base.Domain.Schemas.Interfaces.Responses;

namespace Base.Domain.Schemas;

public class PaginatedResponse<T> : IPaginatedResponse<T>
{
    public long Count { get; set; }
    public long TotalCount { get; set; }
    public IEnumerable<T> Dtos { get; set; }
}