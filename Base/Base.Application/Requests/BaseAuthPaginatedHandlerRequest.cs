using Base.Domain.Helpers.Models;
using Base.Domain.Schemas.Interfaces.Requests;

namespace Base.Application.Requests;

public abstract class BaseAuthPaginatedHandlerRequest<TResponse> : BaseAuthHandlerRequest<TResponse>, IPaginatedRequest
    where TResponse : ApplicationResponse
{
    public int PageNumber { get; init; } = 0;
    
    public int PageSize { get; init; } = 10;
}