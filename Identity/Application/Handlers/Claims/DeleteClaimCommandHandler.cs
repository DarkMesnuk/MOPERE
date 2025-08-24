using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Claims;

public class DeleteClaimCommandHandler(
    ILogger<DeleteClaimCommandHandler> logger, 
    IClaimsRepository claimsRepository
) : BaseHandler<DeleteClaimCommandHandler, DeleteClaimCommandRequest, DeleteClaimCommandResponse>(logger)
{
    public override async Task<DeleteClaimCommandResponse> Handle(DeleteClaimCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new DeleteClaimCommandResponse();
        
        await claimsRepository.DeleteAsync(request.Id);

        return response;
    }
}

public class DeleteClaimCommandRequest : BaseAuthHandlerRequest<DeleteClaimCommandResponse>
{
    public string Id { get; set; } // FromRoute
}

public class DeleteClaimCommandResponse : ApplicationResponse<DeleteClaimCommandResponse>;