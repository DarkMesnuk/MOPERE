using Base.Application;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Identity.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class IsValidCodeCommandHandler(
    ILogger<IsValidCodeCommandHandler> logger,
    IIdentityService identityService
) : BaseHandler<IsValidCodeCommandHandler, IsValidCodeCommandRequest, IsValidCodeCommandResponse>(logger)
{
    public override async Task<IsValidCodeCommandResponse> Handle(IsValidCodeCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new IsValidCodeCommandResponse();
        
        var user = await identityService.GetByEmailOrDefaultAsync(request.Email);

        if (user == null)
        {
            return response.SetData(StatusCodes.InvalidCode);
        }
        
        var isValid = await identityService.IsValidCodeAsync(user, request.Code);

        if (!isValid)
        {
            return response.SetData(StatusCodes.InvalidCode);
        }

        return response;
    }
}

public class IsValidCodeCommandRequest : IRequest<IsValidCodeCommandResponse>
{
    public required string Email { get; init; }
    public required string Code { get; init; }
}

public class IsValidCodeCommandResponse : ApplicationResponse<IsValidCodeCommandResponse>;