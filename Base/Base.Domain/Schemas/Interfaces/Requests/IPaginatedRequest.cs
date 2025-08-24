namespace Base.Domain.Schemas.Interfaces.Requests;

public interface IPaginatedRequest
{
    int PageNumber { get; init; }
    int PageSize { get; init; }
}