using Application.Dtos.Authentication;
using AutoMapper;
using Base.Application;
using Base.Application.Requests;
using Base.Application.Responses;
using Domain.Models.Authentication;
using Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication;

public class RefreshTokenCommandHandler(
    ILogger<RefreshTokenCommandHandler> logger,
    IMapper mapper,
    IIdentityService identityService
) : BaseHandler<RefreshTokenCommandHandler, RefreshTokenCommandRequest, RefreshTokenCommandResponse>(logger)
{
    public override async Task<RefreshTokenCommandResponse> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new RefreshTokenCommandResponse(mapper);

        var token = await identityService.RefreshTokensAsync(request.OldToken, request.RefreshToken);

        return response.SetData(token);
    }
}

public class RefreshTokenCommandRequest : BaseAuthHandlerRequest<RefreshTokenCommandResponse>
{
    public required string OldToken { get; init; }
    public required string RefreshToken { get; init; }
}

public class RefreshTokenCommandResponse(
    IMapper mapper
) : BaseQueryResponse<RefreshTokenCommandResponse, TokenDto, TokenModel>(mapper);