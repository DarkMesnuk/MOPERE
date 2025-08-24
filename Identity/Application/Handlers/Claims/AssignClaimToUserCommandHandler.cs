using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Interfaces.Repositories.Users;
using Domain.Models.Authentication;
using Domain.Models.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Claims;

public class AssignClaimToUserCommandHandler(
    ILogger<AssignClaimToUserCommandHandler> logger,
    IUsersRepository usersRepository,
    IUserClaimsRepository userClaimsRepository,
    IClaimsRepository claimsRepository
) : BaseHandler<AssignClaimToUserCommandHandler, AssignClaimToUserCommandRequest, AssignClaimToUserCommandResponse>(logger)
{
    public override async Task<AssignClaimToUserCommandResponse> Handle(AssignClaimToUserCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new AssignClaimToUserCommandResponse();
        
        await usersRepository.ExistsOrThrowAsync(request.UserId);
        
        await claimsRepository.ExistsOrThrowAsync(request.ClaimId);
        
        await userClaimsRepository.CreateAsync(new UserClaimModel
        {
            User = new UserModel {Id = request.UserId},
            Claim = new  ClaimModel {Id = request.ClaimId},
        });

        return response;
    }
}

public class AssignClaimToUserCommandRequest : BaseAuthHandlerRequest<AssignClaimToUserCommandResponse>
{
    public string ClaimId { get; set; } // FromRoute
    public required string UserId { get; init; }
}

public class AssignClaimToUserCommandResponse : ApplicationResponse<AssignClaimToUserCommandResponse>;