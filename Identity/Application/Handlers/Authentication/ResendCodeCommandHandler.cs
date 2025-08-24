using Base.Application;
using Base.Application.Responses;
using Domain.Interfaces.Infrastructure;
using Identity.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class ResendCodeCommandHandler(
    ILogger<ResendCodeCommandHandler> logger,
    IIdentityService identityService,
    IEmailService emailService
) : BaseHandler<ResendCodeCommandHandler, ResendCodeCommandRequest, ResendCodeCommandResponse>(logger)
{
    public override async Task<ResendCodeCommandResponse> Handle(ResendCodeCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new ResendCodeCommandResponse();
        
        var user = await identityService.GetByEmailOrDefaultAsync(request.Email);

        if (user == null)
        {
            return response;
        }

        await identityService.SetRandomVerificationCodeAndSaveAsync(user);

        await emailService.SendAsync(user.Email!, user.UserName!, "Resend code", $"Your verification code:{user.VerificationCode}");   
        
        return response.SetData(user.VerificationCode!);
    }
}

public class ResendCodeCommandRequest : IRequest<ResendCodeCommandResponse>
{
    public required string Email { get; init; }
}

public class ResendCodeCommandResponse : BaseQueryResponse<ResendCodeCommandResponse, string>;