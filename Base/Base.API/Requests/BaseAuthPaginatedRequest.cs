using Base.Domain.Schemas.Interfaces.Requests;

namespace Base.API.Requests;

public class BaseAuthPaginatedRequest : BaseAuthRequest, IPaginatedRequest
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}