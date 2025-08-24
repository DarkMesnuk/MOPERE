using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Interfaces.Repositories.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Claims;

public class UnassignAllClaimsFromRoleCommandHandler(
    ILogger<UnassignAllClaimsFromRoleCommandHandler> logger, 
    IRolesRepository rolesRepository, 
    IRoleClaimsRepository roleClaimsRepository,
    IUserClaimsRepository userClaimsRepository,
    IUsersRepository usersRepository
) : BaseHandler<UnassignAllClaimsFromRoleCommandHandler, UnassignAllClaimsFromRoleCommandRequest, UnassignAllClaimsFromRoleCommandResponse>(logger)
{
    public override async Task<UnassignAllClaimsFromRoleCommandResponse> Handle(UnassignAllClaimsFromRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new UnassignAllClaimsFromRoleCommandResponse();
        
        await rolesRepository.ExistsOrThrowAsync(request.RoleId);
        
        await roleClaimsRepository.DeleteByRoleAsync(request.RoleId);
        
        var userIds = await usersRepository.GetIdsByRoleAsync(request.RoleId);

        if (userIds.Count > 0)
        {
            await userClaimsRepository.DeleteByUserAsync(userIds);
        }

        return response;
    }
}

public class UnassignAllClaimsFromRoleCommandRequest : BaseAuthHandlerRequest<UnassignAllClaimsFromRoleCommandResponse>
{
    public required string RoleId { get; init; }
}

public class UnassignAllClaimsFromRoleCommandResponse : ApplicationResponse<UnassignAllClaimsFromRoleCommandResponse>;