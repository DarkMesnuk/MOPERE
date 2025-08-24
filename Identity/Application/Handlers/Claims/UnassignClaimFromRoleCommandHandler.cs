using Base.Application;
using Base.Application.Requests;
using Base.Domain.Extensions;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Interfaces.Repositories.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Claims;

public class UnassignClaimFromRoleCommandHandler(
    ILogger<UnassignClaimFromRoleCommandHandler> logger, 
    IRolesRepository rolesRepository, 
    IRoleClaimsRepository roleClaimsRepository,
    IUserClaimsRepository userClaimsRepository,
    IUsersRepository usersRepository
) : BaseHandler<UnassignClaimFromRoleCommandHandler, UnassignClaimFromRoleCommandRequest, UnassignClaimFromRoleCommandResponse>(logger)
{
    public override async Task<UnassignClaimFromRoleCommandResponse> Handle(UnassignClaimFromRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new UnassignClaimFromRoleCommandResponse();
        
        var role = await rolesRepository.GetByIdAsync(request.RoleId);

        var claim = role.Claims?.FirstOrDefault(x => x.Id == request.ClaimId);

        if (claim.IsNullOrDefault())
        {
            return response.SetData(StatusCodes.NotFound)
                .SetAdditionalMessage("Claim in role");
        }

        await roleClaimsRepository.DeleteAsync(role.Id, claim!.Id);
        
        var userIds = await usersRepository.GetIdsByRoleAsync(role.Id);

        if (userIds.Count > 0)
        {
            await userClaimsRepository.DeleteAsync(userIds, claim.Id);
        }

        return response;
    }
}

public class UnassignClaimFromRoleCommandRequest : BaseAuthHandlerRequest<UnassignClaimFromRoleCommandResponse>
{
    public string ClaimId { get; set; } // FromRoute
    public required string RoleId { get; init; }
}

public class UnassignClaimFromRoleCommandResponse : ApplicationResponse<UnassignClaimFromRoleCommandResponse>;