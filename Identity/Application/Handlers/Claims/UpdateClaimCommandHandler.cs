using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Models.Authentication;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Claims;

public class UpdateClaimCommandHandler(
    ILogger<UpdateClaimCommandHandler> logger, 
    IClaimsRepository claimsRepository
) : BaseHandler<UpdateClaimCommandHandler, UpdateClaimCommandRequest, UpdateClaimCommandResponse>(logger)
{
    public override async Task<UpdateClaimCommandResponse> Handle(UpdateClaimCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new UpdateClaimCommandResponse();
        
        var claim = await claimsRepository.GetByIdAsync(request.Id);
        
        if (!IsNewTypeSuccessfullySetIfItNotNull(claim, request.Type))
            return response.SetData(StatusCodes.SomeFieldsMustBeUnique).SetAdditionalMessage("Type");
        
        if (!IsNewValueSuccessfullySetIfItNotNull(claim, request.Value))
            return response.SetData(StatusCodes.SomeFieldsMustBeUnique).SetAdditionalMessage("Value");

        await claimsRepository.UpdateAsync(claim);

        return response;
    }

    private bool IsNewTypeSuccessfullySetIfItNotNull(ClaimModel claim, string? type)
    {
        if (type != null)
        {
            var exists = claimsRepository.Exists(type, claim.Value);
            
            if (exists)
                return false;
            
            claim.Type = type;
        }

        return true;
    }
    
    private bool IsNewValueSuccessfullySetIfItNotNull(ClaimModel claim, string? value)
    {
        if (value != null)
        {
            var existNewValue = claimsRepository.Exists(claim.Type, value);

            if (existNewValue)
                return false;
            
            claim.Value = value;
        }

        return true;
    }
}

public class UpdateClaimCommandRequest : BaseAuthHandlerRequest<UpdateClaimCommandResponse>
{
    public string Id { get; set; } // FromRoute
    public string? Type { get; init; }
    public string? Value { get; init; }
}

public class UpdateClaimCommandResponse : ApplicationResponse<UpdateClaimCommandResponse>;