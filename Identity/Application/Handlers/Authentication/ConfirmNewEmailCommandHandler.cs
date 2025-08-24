using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Users;
using Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class ConfirmNewEmailCommandHandler(
    ILogger<ConfirmNewEmailCommandHandler> logger,
    IIdentityService identityService,
    INewUserEmailsRepository newUserEmailsRepository
) : BaseHandler<ConfirmNewEmailCommandHandler, ConfirmNewEmailCommandRequest, ConfirmNewEmailCommandResponse>(logger)
{
    public override async Task<ConfirmNewEmailCommandResponse> Handle(ConfirmNewEmailCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new ConfirmNewEmailCommandResponse();
        
        var user = await identityService.GetByIdAsync(request.AuthUserId);

        var isValid = await identityService.IsValidCodeAsync(user, request.Code);

        if (!isValid)
        {
            return response.SetData(StatusCodes.InvalidCode);
        }
        
        var newUserEmailModel = await newUserEmailsRepository.GetOrDefaultAsync(user.Id);

        if (newUserEmailModel == null)
        {
            return response.SetData(StatusCodes.SomethingWentWrong, "Time for change email expired");
        }
        
        var newEmail = newUserEmailModel.NewEmail;

        if (await identityService.ExistsByEmailAsync(newEmail))
        {
            return response.SetData(StatusCodes.AlreadyExists)
                .SetFieldErrorMessage(nameof(newEmail), "Email already exists");
        }

        user.Email = newEmail.Trim();
        user.NormalizedEmail = newEmail.Trim().ToUpperInvariant();

        await identityService.UpdateAsync(user);

        return response;
    }
}

public class ConfirmNewEmailCommandRequest : BaseAuthHandlerRequest<ConfirmNewEmailCommandResponse>
{
    public required string Code { get; init; }
}

public class ConfirmNewEmailCommandResponse : ApplicationResponse<ConfirmNewEmailCommandResponse>;