using Base.Application;
using Base.Application.Responses;
using Base.Domain.Helpers;
using Base.Infrastructure.Database.PostgreSQL.Extensions;
using Domain.Interfaces.Infrastructure;
using FluentValidation;
using Identity.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class RegisterCommandHandler(
    ILogger<RegisterCommandHandler> logger,
    IIdentityService identityService,
    IEmailService emailService
) : BaseHandler<RegisterCommandHandler, RegisterCommandRequest, RegisterCommandResponse>(logger)
{
    public override async Task<RegisterCommandResponse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new RegisterCommandResponse();
		
        var userByEmail = await identityService.GetByEmailOrDefaultAsync(request.Email);

        if (userByEmail != null)
        {
            if (!userByEmail.EmailConfirmed)
            {
                var isSuccess = await identityService.DeleteAsync(userByEmail);
                
                if (!isSuccess)
                {
                    return response.SetData(StatusCodes.CreationFailed);   
                }
            }
            else
            {
                return response.SetData(StatusCodes.AlreadyExists)
                    .SetFieldErrorMessage(nameof(request.Email), "Email already exists"); 
            }
        }
        
        var result = await identityService.CreateUserAsync(request.Email, request.Password);
            
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description);
                
            return response
                .SetData(StatusCodes.SomethingWentWrong)
                .SetAdditionalMessage(errors);
        }

        var user = await identityService.GetByEmailOrDefaultAsync(request.Email);

        if (user.IsNullOrDefault())
        {
            return response.SetData(StatusCodes.CreationFailed);   
        }

        await emailService.SendAsync(user!.Email!, user.UserName!, "Confirm registration", $"Your verification code:{user.VerificationCode}");
        
        return response.SetData(user.VerificationCode!);
    }
}

public class RegisterCommandRequest : IRequest<RegisterCommandResponse>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class RegisterCommandResponse : BaseQueryResponse<RegisterCommandResponse, string>;

public class RegisterRequestValidator : AbstractValidator<RegisterCommandRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Password)
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Passwords must have at least one uppercase ('A'-'Z').")
            .Matches(@"[a-z]").WithMessage("Passwords must have at least one lowercase ('a'-'z').")
            .Matches(@"\d").WithMessage("Passwords must have at least one numeric character.")
            .Matches(@"[\W_]").WithMessage("Passwords must have at least one non alphanumeric character.");
    }
}