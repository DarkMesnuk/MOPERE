using Base.Application;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using FluentValidation;
using Identity.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class ResetPasswordCommandHandler(
    ILogger<ResetPasswordCommandHandler> logger,
    IIdentityService identityService
) : BaseHandler<ResetPasswordCommandHandler, ResetPasswordCommandRequest, ResetPasswordCommandResponse>(logger)
{
    public override async Task<ResetPasswordCommandResponse> Handle(ResetPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new ResetPasswordCommandResponse();
        
        var user = await identityService.GetByEmailOrDefaultAsync(request.Email);

        if (user == null)
        {
            return response.SetData(StatusCodes.InvalidCode);
        }

        var isValid = await identityService.IsValidCodeAsync(user, request.Code);

        if (!isValid)
        {
            return response.SetData(StatusCodes.InvalidCode);
        }

        await identityService.ResetPasswordAsync(user, request.NewPassword);

        return response;
    }
}

public class ResetPasswordCommandRequest : IRequest<ResetPasswordCommandResponse>
{
    public required string Email { get; init; }
    public required string NewPassword { get; init; }
    public required string Code { get; init; }
}

public class ResetPasswordCommandResponse : ApplicationResponse<ResetPasswordCommandResponse>;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordCommandRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.NewPassword)
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Passwords must have at least one uppercase ('A'-'Z').")
            .Matches(@"[a-z]").WithMessage("Passwords must have at least one lowercase ('a'-'z').")
            .Matches(@"\d").WithMessage("Passwords must have at least one numeric character.")
            .Matches(@"[\W_]").WithMessage("Passwords must have at least one non alphanumeric character.");
    }
}