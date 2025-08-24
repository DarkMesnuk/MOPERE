using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers.Models;
using Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Roles;

public class AssignRoleToUserCommandHandler(
    ILogger<AssignRoleToUserCommandHandler> logger, 
    IRolesService rolesService
) : BaseHandler<AssignRoleToUserCommandHandler, AssignRoleToUserCommandRequest, AssignRoleToUserCommandResponse>(logger)
{
    public override async Task<AssignRoleToUserCommandResponse> Handle(AssignRoleToUserCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new AssignRoleToUserCommandResponse();
        
        await rolesService.AssignRoleToUserAsync(request.UserId, request.RoleId);

        return response;
    }
}

public class AssignRoleToUserCommandRequest : BaseAuthHandlerRequest<AssignRoleToUserCommandResponse>
{
    public string RoleId { get; set; } // FromRoute
    public required string UserId { get; init; }
}

public class AssignRoleToUserCommandResponse : ApplicationResponse<AssignRoleToUserCommandResponse>;