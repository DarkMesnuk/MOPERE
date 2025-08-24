using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Models.Authentication;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Roles;

public class CreateRoleCommandHandler(
    ILogger<CreateRoleCommandHandler> logger, 
    IRolesRepository rolesRepository
) : BaseHandler<CreateRoleCommandHandler, CreateRoleCommandRequest, CreateRoleCommandResponse>(logger)
{
    public override async Task<CreateRoleCommandResponse> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new CreateRoleCommandResponse();
        
        var exists = await rolesRepository.ExistsByNameAsync(request.Name);

        if (exists)
        {
            return response.SetData(StatusCodes.SomeFieldsMustBeUnique).SetAdditionalMessage("Name");
        }

        var role = new RoleModel
        {
            Name = request.Name
        };

        await rolesRepository.CreateAsync(role);

        return response;
    }
}

public class CreateRoleCommandRequest : BaseAuthHandlerRequest<CreateRoleCommandResponse>
{
    public required string Name { get; init; }
}

public class CreateRoleCommandResponse : ApplicationResponse<CreateRoleCommandResponse>;