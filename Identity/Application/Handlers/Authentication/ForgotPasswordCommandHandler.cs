using Base.Application;
using Base.Application.Responses;
using Domain.Interfaces.Infrastructure;
using Identity.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class ForgotPasswordCommandHandler(
    ILogger<ForgotPasswordCommandHandler> logger,
    IEmailService emailService,
    IIdentityService identityService
) : BaseHandler<ForgotPasswordCommandHandler, ForgotPasswordCommandRequest, ForgotPasswordCommandResponse>(logger)
{
    public override async Task<ForgotPasswordCommandResponse> Handle(ForgotPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new ForgotPasswordCommandResponse();
        
        var user = await identityService.GetByEmailOrDefaultAsync(request.Email);

        if (user == null)
        {
            return response;
        }

        await identityService.SetRandomVerificationCodeAndSaveAsync(user);
        
        await emailService.SendAsync(request.Email, user.UserName!, "Forgot Password", $"Your verification code:{user.VerificationCode}");
        
        return response.SetData(user.VerificationCode!);
    }
}

public class ForgotPasswordCommandRequest : IRequest<ForgotPasswordCommandResponse>
{
    public required string Email { get; init; }
}

public class ForgotPasswordCommandResponse : BaseQueryResponse<ForgotPasswordCommandResponse, string>;