using Base.Domain.Helpers.Models;
using MediatR;

namespace Base.Application.Requests;

public abstract class BaseAuthHandlerRequest<TResponse> : BaseAuthHandlerRequest, IRequest<TResponse>
    where TResponse : ApplicationResponse;


public abstract class BaseAuthHandlerRequest
{
    public string AuthUserId { get; init; }   
}

