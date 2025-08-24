using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Roles;

public class DeleteRoleCommandHandler(
    ILogger<DeleteRoleCommandHandler> logger, 
    IRolesRepository rolesRepository
) : BaseHandler<DeleteRoleCommandHandler, DeleteRoleCommandRequest, DeleteRoleCommandResponse>(logger)
{
    public override async Task<DeleteRoleCommandResponse> Handle(DeleteRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new DeleteRoleCommandResponse();
        
        await rolesRepository.DeleteAsync(request.Id);

        return response;
    }
}

public class DeleteRoleCommandRequest : BaseAuthHandlerRequest<DeleteRoleCommandResponse>
{
    public string Id { get; set; } // FromRoute
}

public class DeleteRoleCommandResponse : ApplicationResponse<DeleteRoleCommandResponse>;