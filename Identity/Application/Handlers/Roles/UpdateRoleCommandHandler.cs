using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Models.Authentication;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Roles;

public class UpdateRoleCommandHandler(
    ILogger<UpdateRoleCommandHandler> logger, 
    IRolesRepository rolesRepository
) : BaseHandler<UpdateRoleCommandHandler, UpdateRoleCommandRequest, UpdateRoleCommandResponse>(logger)
{
    public override async Task<UpdateRoleCommandResponse> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new UpdateRoleCommandResponse();
        
        var role = await rolesRepository.GetByIdAsync(request.Id);
        
        if (!await IsNewNameSuccessfullySetIfItNotNull(role, request.Name))
            return response.SetData(StatusCodes.SomeFieldsMustBeUnique).SetAdditionalMessage("Name");
        
        await rolesRepository.UpdateAsync(role);

        return response;
    }
    
    private async Task<bool> IsNewNameSuccessfullySetIfItNotNull(RoleModel role, string? name)
    {
        if (name == null)
            return true;

        var exists = await rolesRepository.ExistsByNameAsync(name);
            
        if (exists)
            return false;
            
        role.Name = name;

        return true;
    }
}

public class UpdateRoleCommandRequest : BaseAuthHandlerRequest<UpdateRoleCommandResponse>
{
    public string Id { get; set; } // FromRoute
    public string? Name { get; init; }
}

public class UpdateRoleCommandResponse : ApplicationResponse<UpdateRoleCommandResponse>;