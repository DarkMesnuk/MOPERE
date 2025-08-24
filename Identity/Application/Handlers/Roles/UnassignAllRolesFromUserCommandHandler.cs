using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Interfaces.Repositories.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Roles;

public class UnassignAllRolesFromUserCommandHandler(
    ILogger<UnassignAllRolesFromUserCommandHandler> logger, 
    IUserClaimsRepository userClaimsRepository,
    IUserRolesRepository userRolesRepository,
    IUsersRepository usersRepository
) : BaseHandler<UnassignAllRolesFromUserCommandHandler, UnassignAllRolesFromUserCommandRequest, UnassignAllRolesFromUserCommandResponse>(logger)
{
    public override async Task<UnassignAllRolesFromUserCommandResponse> Handle(UnassignAllRolesFromUserCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new UnassignAllRolesFromUserCommandResponse();
        
        await usersRepository.ExistsOrThrowAsync(request.UserId);
        
        await userRolesRepository.DeleteByUserAsync(request.UserId);
        
        await userClaimsRepository.DeleteByUserAsync(request.UserId);

        return response;
    }
}

public class UnassignAllRolesFromUserCommandRequest : BaseAuthHandlerRequest<UnassignAllRolesFromUserCommandResponse>
{
    public required string UserId { get; init; }
}

public class UnassignAllRolesFromUserCommandResponse : ApplicationResponse<UnassignAllRolesFromUserCommandResponse>;