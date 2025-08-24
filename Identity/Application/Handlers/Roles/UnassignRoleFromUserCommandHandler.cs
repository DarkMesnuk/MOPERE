using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Interfaces.Repositories.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Roles;

public class UnassignRoleFromUserCommandHandler(
    ILogger<UnassignRoleFromUserCommandHandler> logger, 
    IUserClaimsRepository userClaimsRepository,
    IUserRolesRepository userRolesRepository,
    IUsersRepository usersRepository,
    IRolesRepository rolesRepository
) : BaseHandler<UnassignRoleFromUserCommandHandler, UnassignRoleFromUserCommandRequest, UnassignRoleFromUserCommandResponse>(logger)
{
    public override async Task<UnassignRoleFromUserCommandResponse> Handle(UnassignRoleFromUserCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new UnassignRoleFromUserCommandResponse();
        
        await usersRepository.ExistsOrThrowAsync(request.UserId);

        await rolesRepository.ExistsOrThrowAsync(request.RoleId);
        
        await userRolesRepository.DeleteAsync(request.UserId, request.RoleId);
        
        await userClaimsRepository.DeleteByUserAsync(request.UserId);

        return response;
    }
}

public class UnassignRoleFromUserCommandRequest : BaseAuthHandlerRequest<UnassignRoleFromUserCommandResponse>
{
    public string RoleId { get; set; } // FromRoute
    public required string UserId { get; init; }
}

public class UnassignRoleFromUserCommandResponse : ApplicationResponse<UnassignRoleFromUserCommandResponse>;