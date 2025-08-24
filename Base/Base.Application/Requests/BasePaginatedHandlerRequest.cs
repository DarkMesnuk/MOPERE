using Base.Domain.Helpers.Models;
using Base.Domain.Schemas.Interfaces.Requests;
using MediatR;

namespace Base.Application.Requests;

public abstract class BasePaginatedHandlerRequest<TResponse> : IRequest<TResponse>, IPaginatedRequest
    where TResponse : ApplicationResponse
{
    public int PageNumber { get; init; } = 0;
    
    public int PageSize { get; init; } = 10;
}