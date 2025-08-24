using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Interfaces.Repositories.Users;
using Domain.Models.Authentication;
using Domain.Models.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Claims;

public class SetClaimsToUserCommandHandler(
    ILogger<SetClaimsToUserCommandHandler> logger, 
    IClaimsRepository claimsRepository,
    IUserClaimsRepository userClaimsRepository,
    IUsersRepository usersRepository
) : BaseHandler<SetClaimsToUserCommandHandler, SetClaimsToUserCommandRequest, SetClaimsToUserCommandResponse>(logger)
{
    public override async Task<SetClaimsToUserCommandResponse> Handle(SetClaimsToUserCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new SetClaimsToUserCommandResponse();
        
        await usersRepository.ExistsOrThrowAsync(request.UserId);

        var claims = await claimsRepository.GetByIdsAsync(request.ClaimIds);

        if (claims.Count != request.ClaimIds.Count)
        {
            return response.SetData(StatusCodes.NotFound)
                .SetAdditionalMessage("Some claims");
        }

        var userClaims = await userClaimsRepository.GetByUserAsync(request.UserId);

        var assignClaims = userClaims.Select(x => x.Claim).ToList();
        
        var claimsToUnassign = assignClaims
            .Where(x => !ClaimContains(claims, x))
            .ToList();

        if (claimsToUnassign.Count > 0)
        {
            await userClaimsRepository.DeleteAsync(request.UserId, claimsToUnassign.Select(x => x.Id).ToList());   
        }
        
        var claimsToAssign = claims
            .Where(x => !ClaimContains(assignClaims, x))
            .ToList();

        if (claimsToAssign.Count > 0)
        {
            await userClaimsRepository.CreateAsync(claimsToAssign.Select(c => new UserClaimModel
            {
                User = new UserModel { Id = request.UserId },
                Claim = c,
            }));
        }

        return response;
    }

    private bool ClaimContains(IEnumerable<ClaimModel> claims, ClaimModel targetClaim)
    {
        return claims.Any(x => x.Id == targetClaim.Id);
    }
}

public class SetClaimsToUserCommandRequest : BaseAuthHandlerRequest<SetClaimsToUserCommandResponse>
{
    public required string UserId { get; init; }
    public required ICollection<string> ClaimIds { get; init; }
}

public class SetClaimsToUserCommandResponse : ApplicationResponse<SetClaimsToUserCommandResponse>;