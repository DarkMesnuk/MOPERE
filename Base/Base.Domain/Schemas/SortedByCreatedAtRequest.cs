using Base.Domain.Enums;
using Base.Domain.Schemas.Interfaces.Requests;

namespace Base.Domain.Schemas;

public class SortedByCreatedAtRequest : ISortedRequest
{
    public string? Sort { get; init; } = "CreatedAt";
    public SortDirection? SortDirection { get; init; } = Base.Domain.Enums.SortDirection.Desc;
}