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

public class SetClaimsToRoleCommandHandler(
    ILogger<SetClaimsToRoleCommandHandler> logger, 
    IRolesRepository rolesRepository,
    IClaimsRepository claimsRepository, 
    IRoleClaimsRepository roleClaimsRepository,
    IUserClaimsRepository userClaimsRepository,
    IUsersRepository usersRepository
) : BaseHandler<SetClaimsToRoleCommandHandler, SetClaimsToRoleCommandRequest, SetClaimsToRoleCommandResponse>(logger)
{
    public override async Task<SetClaimsToRoleCommandResponse> Handle(SetClaimsToRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new SetClaimsToRoleCommandResponse();
        
        await rolesRepository.ExistsOrThrowAsync(request.RoleId);

        var claims = await claimsRepository.GetByIdsAsync(request.ClaimIds);

        if (claims.Count != request.ClaimIds.Count)
        {
            return response.SetData(StatusCodes.NotFound)
                .SetAdditionalMessage("Some claims");
        }

        var roleClaims = await roleClaimsRepository.GetByRoleAsync(request.RoleId);
        
        var userIds = await usersRepository.GetIdsByRoleAsync(request.RoleId);

        var assignClaims = roleClaims.Select(x => x.Claim!).ToList();
        
        var claimsToUnassign = assignClaims
            .Where(x => !ClaimContains(claims, x))
            .ToList();
        
        if (claimsToUnassign.Count > 0)
        {
            await roleClaimsRepository.DeleteAsync(request.RoleId, claimsToUnassign.Select(x => x.Id).ToList());   
            await userClaimsRepository.DeleteAsync(userIds, claimsToUnassign.Select(x => x.Id).ToList());  
        }
        
        var claimsToAssign = claims
            .Where(x => !ClaimContains(assignClaims, x))
            .ToList();
        
        if (claimsToAssign.Count > 0)
        {
            await roleClaimsRepository.CreateAsync(claimsToAssign.Select(c => new RoleClaimModel
            {
                Role = new RoleModel { Id = request.RoleId },
                Claim = c,
            }));
            await userClaimsRepository.CreateAsync(userIds.SelectMany(x => claimsToAssign.Select(c => new UserClaimModel
            {
                User = new UserModel { Id = x },
                Claim = c,
            })));
        }

        return response;
    }

    private bool ClaimContains(IEnumerable<ClaimModel> claims, ClaimModel targetClaim)
    {
        return claims.Any(x => x.Id == targetClaim.Id);
    }
}

public class SetClaimsToRoleCommandRequest : BaseAuthHandlerRequest<SetClaimsToRoleCommandResponse>
{
    public required string RoleId { get; init; }
    public required ICollection<string> ClaimIds { get; init; }
}

public class SetClaimsToRoleCommandResponse : ApplicationResponse<SetClaimsToRoleCommandResponse>;