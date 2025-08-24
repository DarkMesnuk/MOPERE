using Application.Dtos.Authentication;
using AutoMapper;
using Base.Application;
using Base.Application.Responses;
using Base.Domain.Helpers;
using Domain.Models.Authentication;
using Identity.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class ConfirmRegistrationCommandHandler(
    ILogger<ConfirmRegistrationCommandHandler> logger,
    IMapper mapper,
    IIdentityService identityService
) : BaseHandler<ConfirmRegistrationCommandHandler, ConfirmRegistrationCommandRequest, ConfirmRegistrationCommandResponse>(logger)
{
    public override async Task<ConfirmRegistrationCommandResponse> Handle(ConfirmRegistrationCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new ConfirmRegistrationCommandResponse(mapper);
        
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
        
        if (user.EmailConfirmed)
        {
            return response.SetData(StatusCodes.SomethingWentWrong, "Account has already been registered");
        }

        await identityService.ConfirmEmailAsync(user);
        
        var token = await identityService.GetTokenAsync(user);
        
        return response.SetData(token);
    }
}

public class ConfirmRegistrationCommandRequest : IRequest<ConfirmRegistrationCommandResponse>
{
    public required string Email { get; init; }
    public required string Code { get; init; }
}

public class ConfirmRegistrationCommandResponse(
    IMapper mapper
) : BaseQueryResponse<ConfirmRegistrationCommandResponse, TokenDto, TokenModel>(mapper);