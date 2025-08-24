using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Interfaces.Repositories.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Claims;

public class UnassignClaimFromUserCommandHandler(
    ILogger<UnassignClaimFromUserCommandHandler> logger,
    IUsersRepository usersRepository,
    IUserClaimsRepository userClaimsRepository,
    IClaimsRepository claimsRepository
) : BaseHandler<UnassignClaimFromUserCommandHandler, UnassignClaimFromUserCommandRequest, UnassignClaimFromUserCommandResponse>(logger)
{
    public override async Task<UnassignClaimFromUserCommandResponse> Handle(UnassignClaimFromUserCommandRequest request, CancellationToken cancellationFromken)
    {
        var response = new UnassignClaimFromUserCommandResponse();
        
        await usersRepository.ExistsOrThrowAsync(request.UserId);
        
        await claimsRepository.ExistsOrThrowAsync(request.ClaimId);
        
        await userClaimsRepository.DeleteAsync(request.UserId, request.ClaimId);

        return response;
    }
}

public class UnassignClaimFromUserCommandRequest : BaseAuthHandlerRequest<UnassignClaimFromUserCommandResponse>
{
    public string ClaimId { get; set; } // FromRoute
    public required string UserId { get; init; }
}

public class UnassignClaimFromUserCommandResponse : ApplicationResponse<UnassignClaimFromUserCommandResponse>;