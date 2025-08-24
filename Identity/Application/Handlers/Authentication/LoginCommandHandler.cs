using Application.Dtos.Authentication;
using AutoMapper;
using Base.Application;
using Base.Application.Responses;
using Base.Domain.Exceptions;
using Base.Domain.Helpers;
using Base.Infrastructure.Database.PostgreSQL.Extensions;
using Domain.Models.Authentication;
using Identity.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class LoginCommandHandler(
    ILogger<LoginCommandHandler> logger, 
    IMapper mapper,
    IIdentityService identityService
) : BaseHandler<LoginCommandHandler, LoginCommandRequest, LoginCommandResponse>(logger)
{
    public override async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new LoginCommandResponse(mapper);

        var user = await identityService.GetByEmailOrDefaultIfEvenDeletedAsync(request.Email);

        if (user.IsNullOrDefault())
        {
            return response.SetData(StatusCodes.IncorrectCredentials); 
        }

        if (user.IsDeleted)
        {
            return response.SetData(StatusCodes.AccountDeleted);
        }

        if (user.IsBlocked)
        {
            throw new BlockedException();
        }

        var checkPasswordResult = await identityService.CheckPasswordAsync(user, request.Password);

        if (!checkPasswordResult.Succeeded)
        {
            return response.SetData(StatusCodes.IncorrectCredentials); 
        }
        
        if (!user.EmailConfirmed)
        {
            return response.SetData(StatusCodes.SomethingWentWrong, "Need to confirm email");
        }
        
        var token = await identityService.GetTokenAsync(user);
        
        return response.SetData(token);
    }
}

public class LoginCommandRequest : IRequest<LoginCommandResponse>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class LoginCommandResponse(
    IMapper mapper
) : BaseQueryResponse<LoginCommandResponse, TokenDto, TokenModel>(mapper);