using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Interfaces.Repositories.Users;
using Domain.Models.Authentication;
using Domain.Models.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Claims;

public class AssignClaimToRoleCommandHandler(
    ILogger<AssignClaimToRoleCommandHandler> logger,
    IRolesRepository rolesRepository,
    IClaimsRepository claimsRepository,
    IRoleClaimsRepository roleClaimsRepository,
    IUserClaimsRepository userClaimsRepository,
    IUsersRepository usersRepository
) : BaseHandler<AssignClaimToRoleCommandHandler, AssignClaimToRoleCommandRequest, AssignClaimToRoleCommandResponse>(logger)
{
    public override async Task<AssignClaimToRoleCommandResponse> Handle(AssignClaimToRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new AssignClaimToRoleCommandResponse();

        await rolesRepository.ExistsOrThrowAsync(request.RoleId);

        await claimsRepository.ExistsOrThrowAsync(request.ClaimId);

        var claim = new ClaimModel { Id = request.ClaimId };
        
        await roleClaimsRepository.CreateAsync(new RoleClaimModel
        {
            Role = new RoleModel { Id = request.RoleId },
            Claim = claim
        });
        
        var userIds = await usersRepository.GetIdsByRoleAsync(request.RoleId);

        if (userIds.Count > 0)
        {
            await userClaimsRepository.CreateAsync(userIds.Select(x => new UserClaimModel
            {
                User = new UserModel { Id = x },
                Claim = claim,
            }));
        }

        return response;
    }
}

public class AssignClaimToRoleCommandRequest : BaseAuthHandlerRequest<AssignClaimToRoleCommandResponse>
{
    public string ClaimId { get; set; } // FromRoute
    public required string RoleId { get; init; }
}

public class AssignClaimToRoleCommandResponse : ApplicationResponse<AssignClaimToRoleCommandResponse>;