using MediatR;
using Microsoft.Extensions.Logging;

namespace Base.Application;

public abstract class BaseHandler<THandler, TRequest, TResponse>(
    ILogger<BaseHandler<THandler, TRequest, TResponse>> logger
) : IRequestHandler<TRequest, TResponse>
    where THandler : class
    where TRequest : class, IRequest<TResponse>
    where TResponse : class
{
    protected readonly ILogger<BaseHandler<THandler, TRequest, TResponse>> Logger = logger;

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}