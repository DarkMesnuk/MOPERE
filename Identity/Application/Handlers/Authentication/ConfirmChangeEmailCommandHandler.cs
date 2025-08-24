using Base.Application;
using Base.Application.Requests;
using Base.Application.Responses;
using Base.Domain.Helpers;
using Domain.Interfaces.Infrastructure;
using Domain.Interfaces.Repositories.Users;
using Domain.Models.Users;
using Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class ConfirmChangeEmailCommandHandler(
    ILogger<ConfirmChangeEmailCommandHandler> logger,
    IIdentityService identityService,
    INewUserEmailsRepository newUserEmailsRepository,
    IEmailService emailService
) : BaseHandler<ConfirmChangeEmailCommandHandler, ConfirmChangeEmailCommandRequest, ConfirmChangeEmailCommandResponse>(logger)
{
    public override async Task<ConfirmChangeEmailCommandResponse> Handle(ConfirmChangeEmailCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new ConfirmChangeEmailCommandResponse();
        
        var user = await identityService.GetByIdAsync(request.AuthUserId);

        var isValid = await identityService.IsValidCodeAsync(user, request.Code);

        if (!isValid)
        {
            return response.SetData(StatusCodes.InvalidCode);
        }

        if (await identityService.ExistsByEmailAsync(request.NewEmail))
        {
            return response.SetData(StatusCodes.AlreadyExists)
                .SetFieldErrorMessage(nameof(request.NewEmail), $"Email already exists");
        }
        
        await identityService.SetRandomVerificationCodeAndSaveAsync(user);
        
        await emailService.SendAsync(request.NewEmail, user.UserName!, "Confirm new email", $"Your verification code:{user.VerificationCode}");  
        
        await newUserEmailsRepository.UpdateOrCreateAsync(user.Id, new NewUserEmailModel { NewEmail = request.NewEmail});

        return response.SetData(user.VerificationCode!);
    }
}

public class ConfirmChangeEmailCommandRequest : BaseAuthHandlerRequest<ConfirmChangeEmailCommandResponse>
{
    public required string NewEmail { get; init; }
    public required string Code { get; init; }
}

public class ConfirmChangeEmailCommandResponse : BaseQueryResponse<ConfirmChangeEmailCommandResponse, string>;