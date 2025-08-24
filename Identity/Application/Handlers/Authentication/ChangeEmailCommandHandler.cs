using Base.Application;
using Base.Application.Requests;
using Base.Application.Responses;
using Base.Domain.Helpers;
using Domain.Interfaces.Infrastructure;
using Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class ChangeEmailCommandHandler(
    ILogger<ChangeEmailCommandHandler> logger,
    IIdentityService identityService,
    IEmailService emailService
) : BaseHandler<ChangeEmailCommandHandler, ChangeEmailCommandRequest, ChangeEmailCommandResponse>(logger)
{
    public override async Task<ChangeEmailCommandResponse> Handle(ChangeEmailCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new ChangeEmailCommandResponse();
        
        var user = await identityService.GetByIdAsync(request.AuthUserId);

        if (!string.Equals(user.NormalizedEmail, request.Email, StringComparison.CurrentCultureIgnoreCase))
        {
            return response.SetData(StatusCodes.SomethingWentWrong)
                .SetFieldErrorMessage(nameof(request.Email), "It`s not your email");
        }
        
        await identityService.SetRandomVerificationCodeAndSaveAsync(user);
        
        await emailService.SendAsync(user.Email!, user.UserName!, "Change email", $"Your verification code:{user.VerificationCode}");   
        
        return response.SetData(user.VerificationCode!);
    }
}

public class ChangeEmailCommandRequest : BaseAuthHandlerRequest<ChangeEmailCommandResponse>
{
    public required string Email { get; init; }
}

public class ChangeEmailCommandResponse : BaseQueryResponse<ChangeEmailCommandResponse, string>;