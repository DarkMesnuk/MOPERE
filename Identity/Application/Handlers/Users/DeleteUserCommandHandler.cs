using Base.Application;
using Base.Application.Requests;
using Base.Application.Responses;
using Domain.Interfaces.Infrastructure;
using Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Users;

public class DeleteUserCommandHandler(
    ILogger<DeleteUserCommandHandler> logger,
    IIdentityService identityService,
    IEmailService emailService   
) : BaseHandler<DeleteUserCommandHandler, DeleteUserCommandRequest, DeleteUserCommandResponse>(logger)
{
    public override async Task<DeleteUserCommandResponse> Handle(DeleteUserCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new DeleteUserCommandResponse();
        
        var user = await identityService.GetByIdAsync(request.AuthUserId);
        
        await identityService.SetRandomVerificationCodeAndSaveAsync(user);
        
        await emailService.SendAsync(user.Email!, user.UserName!, "Resend code", $"Your verification code:{user.VerificationCode}");   
        
        return response.SetData(user.VerificationCode!);
    }
}

public class DeleteUserCommandRequest : BaseAuthHandlerRequest<DeleteUserCommandResponse>;

public class DeleteUserCommandResponse : BaseQueryResponse<DeleteUserCommandResponse, string>;