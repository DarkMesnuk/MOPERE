using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using FluentValidation;
using Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class ChangePasswordCommandHandler(
	ILogger<ChangePasswordCommandHandler> logger, 
	IIdentityService identityService
) : BaseHandler<ChangePasswordCommandHandler, ChangePasswordCommandRequest, ChangePasswordCommandResponse>(logger)
{
	public override async Task<ChangePasswordCommandResponse> Handle(ChangePasswordCommandRequest request, CancellationToken cancellationToken)
	{
		var response = new ChangePasswordCommandResponse();
		
		var user = await identityService.GetByIdAsync(request.AuthUserId);

		var checkPasswordResult = await identityService.CheckPasswordAsync(user, request.OldPassword);

		if (!checkPasswordResult.Succeeded)
		{
			return response.SetData(StatusCodes.IncorrectCredentials);
		}

		await identityService.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

		return response;
	}
}

public class ChangePasswordCommandRequest : BaseAuthHandlerRequest<ChangePasswordCommandResponse>
{
	public required string NewPassword { get; init; }
	public required string OldPassword { get; init; }
}

public class ChangePasswordCommandResponse : ApplicationResponse<ChangePasswordCommandResponse>;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordCommandRequest>
{
	public ChangePasswordRequestValidator()
	{
		RuleFor(x => x.NewPassword)
			.MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
			.Matches(@"[A-Z]").WithMessage("Passwords must have at least one uppercase ('A'-'Z').")
			.Matches(@"[a-z]").WithMessage("Passwords must have at least one lowercase ('a'-'z').")
			.Matches(@"\d").WithMessage("Passwords must have at least one numeric character.")
			.Matches(@"[\W_]").WithMessage("Passwords must have at least one non alphanumeric character.");
	}
}