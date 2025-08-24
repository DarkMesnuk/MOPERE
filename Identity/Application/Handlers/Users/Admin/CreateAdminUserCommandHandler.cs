using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Users;
using Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Users.Admin;

public class CreateAdminUserCommandHandler(
    ILogger<CreateAdminUserCommandHandler> logger,
    IIdentityService identityService,
    IRolesService rolesService,
    IUsersRepository usersRepository
) : BaseHandler<CreateAdminUserCommandHandler, CreateAdminUserCommandRequest, CreateAdminUserCommandResponse>(logger)
{
    public override async Task<CreateAdminUserCommandResponse> Handle(CreateAdminUserCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new CreateAdminUserCommandResponse();
        
        var result = await identityService.CreateUserAsync(request.Email, request.Password);
        
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description);
                
            return response
                .SetData(StatusCodes.SomethingWentWrong)
                .SetAdditionalMessage(errors);
        }
            
        var user = await usersRepository.GetByEmailAsync(request.Email);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        await usersRepository.UpdateAsync(user);
        
        await rolesService.AssignRoleToUserAsync(user.Id, request.RoleId);
        
        return response;
    }
}

public class CreateAdminUserCommandRequest : BaseAuthHandlerRequest<CreateAdminUserCommandResponse>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    
    public required string RoleId { get; init; }
}

public class CreateAdminUserCommandResponse : ApplicationResponse<CreateAdminUserCommandResponse>;