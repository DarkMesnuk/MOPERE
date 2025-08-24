using Base.Application;
using Base.Application.Requests;
using Base.Domain.Extensions;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Models.Authentication;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Claims;

public class CreateClaimCommandHandler(
    ILogger<CreateClaimCommandHandler> logger, 
    IClaimsRepository claimsRepository
) : BaseHandler<CreateClaimCommandHandler, CreateClaimCommandRequest, CreateClaimCommandResponse>(logger)
{
    public override async Task<CreateClaimCommandResponse> Handle(CreateClaimCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new CreateClaimCommandResponse();
        
        var exists = claimsRepository.Exists(request.Type, request.Value);

        if (exists)
        {
            return response.SetData(StatusCodes.AlreadyExists);
        }

        var claim = new ClaimModel
        {
            Type = request.Type.ToNormalized(),
            Value = request.Value.ToNormalized()
        };

        await claimsRepository.CreateAsync(claim);

        return response;
    }
}

public class CreateClaimCommandRequest : BaseAuthHandlerRequest<CreateClaimCommandResponse>
{
    public required string Type { get; init; }
    public required string Value { get; init; }
}

public class CreateClaimCommandResponse : ApplicationResponse<CreateClaimCommandResponse>;