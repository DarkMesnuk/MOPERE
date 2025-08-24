using Base.Application;
using Base.Application.Requests;
using Base.Application.Responses;
using Domain.Interfaces.Infrastructure;
using Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class RestoreUserCommandHandler(
    ILogger<RestoreUserCommandHandler> logger,
    IIdentityService identityService,
    IEmailService emailService
) : BaseHandler<RestoreUserCommandHandler, RestoreUserCommandRequest, RestoreUserCommandResponse>(logger)
{
    public override async Task<RestoreUserCommandResponse> Handle(RestoreUserCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new RestoreUserCommandResponse();
        
        var user = await identityService.GetByEmailOrDefaultIfEvenDeletedAsync(request.Email);

        if (user == null)
        {
            return response;
        }

        await identityService.SetRandomVerificationCodeAndSaveAsync(user);

        await emailService.SendAsync(user.Email!, user.UserName!, "Restore code", $"Your verification code:{user.VerificationCode}");   

        return response.SetData(user.VerificationCode!);
    }
}

public class RestoreUserCommandRequest : BaseAuthHandlerRequest<RestoreUserCommandResponse>
{
    public required string Email { get; init; }
}

public class RestoreUserCommandResponse : BaseQueryResponse<RestoreUserCommandResponse, string>;