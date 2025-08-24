using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class ConfirmRestoreUserCommandHandler(
    ILogger<ConfirmRestoreUserCommandHandler> logger,
    IIdentityService identityService
) : BaseHandler<ConfirmRestoreUserCommandHandler, ConfirmRestoreUserCommandRequest, ConfirmRestoreUserCommandResponse>(logger)
{
    public override async Task<ConfirmRestoreUserCommandResponse> Handle(ConfirmRestoreUserCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new ConfirmRestoreUserCommandResponse();
        
        var user = await identityService.GetByEmailOrDefaultIfEvenDeletedAsync(request.Email);

        if (user == null)
        {
            return response.SetData(StatusCodes.InvalidCode);
        }
        
        var isValid = await identityService.IsValidCodeAsync(user, request.Code);

        if (!isValid)
        {
            return response.SetData(StatusCodes.InvalidCode);
        }
        
        user.IsDeleted = false;
        
        await identityService.UpdateAsync(user);

        return response;
    }
}

public class ConfirmRestoreUserCommandRequest : BaseAuthHandlerRequest<ConfirmRestoreUserCommandResponse>
{
    public required string Email { get; init; }
    public required string Code { get; init; }
}

public class ConfirmRestoreUserCommandResponse : ApplicationResponse<ConfirmRestoreUserCommandResponse>;