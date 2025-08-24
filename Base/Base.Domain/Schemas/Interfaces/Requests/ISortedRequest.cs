using Base.Domain.Enums;

namespace Base.Domain.Schemas.Interfaces.Requests;

public interface ISortedRequest
{
    string? Sort { get; init; }
    SortDirection? SortDirection { get; init; }
}