using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Users;

public class ConfirmDeleteUserCommandHandler(
    ILogger<ConfirmDeleteUserCommandHandler> logger,
    IIdentityService identityService
) : BaseHandler<ConfirmDeleteUserCommandHandler, ConfirmDeleteUserCommandRequest, ConfirmDeleteUserCommandResponse>(logger)
{
    public override async Task<ConfirmDeleteUserCommandResponse> Handle(ConfirmDeleteUserCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new ConfirmDeleteUserCommandResponse();

        var user = await identityService.GetByIdAsync(request.AuthUserId);

        var isValid = await identityService.IsValidCodeAsync(user, request.Code);

        if (!isValid)
        {
            return response.SetData(StatusCodes.InvalidCode);
        }
        
        await identityService.TemporaryDeleteAsync(user);

        return response;
    }
}

public class ConfirmDeleteUserCommandRequest : BaseAuthHandlerRequest<ConfirmDeleteUserCommandResponse>
{
    public required string Code { get; init; }
}

public class ConfirmDeleteUserCommandResponse : ApplicationResponse<ConfirmDeleteUserCommandResponse>;