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

public class AssignClaimsToRoleCommandHandler(
    ILogger<AssignClaimsToRoleCommandHandler> logger, 
    IRolesRepository rolesRepository, 
    IRoleClaimsRepository roleClaimsRepository,
    IClaimsRepository claimsRepository,
    IUserClaimsRepository userClaimsRepository,
    IUsersRepository usersRepository
) : BaseHandler<AssignClaimsToRoleCommandHandler, AssignClaimsToRoleCommandRequest, AssignClaimsToRoleCommandResponse>(logger)
{
    public override async Task<AssignClaimsToRoleCommandResponse> Handle(AssignClaimsToRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new AssignClaimsToRoleCommandResponse();
        
        await rolesRepository.ExistsOrThrowAsync(request.RoleId);

        var claims = await claimsRepository.GetByIdsAsync(request.ClaimIds);

        if (claims.Count != request.ClaimIds.Count)
        {
            return response.SetData(StatusCodes.NotFound)
                .SetAdditionalMessage("Some claims");
        }
        
        await roleClaimsRepository.CreateAsync(claims.Select(x => new RoleClaimModel
        {
            Role = new RoleModel { Id = request.RoleId},
            Claim = x
        }));
        
        var userIds = await usersRepository.GetIdsByRoleAsync(request.RoleId);

        if (userIds.Count > 0)
        {
            await userClaimsRepository.CreateAsync(userIds.SelectMany(x => claims.Select(c => new UserClaimModel
            {
                User = new UserModel { Id = x },
                Claim = c,
            })));
        }

        return response;
    }
}

public class AssignClaimsToRoleCommandRequest : BaseAuthHandlerRequest<AssignClaimsToRoleCommandResponse>
{
    public required string RoleId { get; init; }
    public required ICollection<string> ClaimIds { get; init; }
}

public class AssignClaimsToRoleCommandResponse : ApplicationResponse<AssignClaimsToRoleCommandResponse>;