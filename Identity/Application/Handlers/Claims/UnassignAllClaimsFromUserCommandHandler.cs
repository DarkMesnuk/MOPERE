using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Interfaces.Repositories.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Claims;

public class UnassignAllClaimsFromUserCommandHandler(
    ILogger<UnassignAllClaimsFromUserCommandHandler> logger, 
    IUsersRepository usersRepository, 
    IUserClaimsRepository userClaimsRepository
) : BaseHandler<UnassignAllClaimsFromUserCommandHandler, UnassignAllClaimsFromUserCommandRequest, UnassignAllClaimsFromUserCommandResponse>(logger)
{
    public override async Task<UnassignAllClaimsFromUserCommandResponse> Handle(UnassignAllClaimsFromUserCommandRequest request, CancellationToken cancellationFromken)
    {
        var response = new UnassignAllClaimsFromUserCommandResponse();
        
        await usersRepository.ExistsOrThrowAsync(request.UserId);
        
        await userClaimsRepository.DeleteByUserAsync(request.UserId);

        return response;
    }
}

public class UnassignAllClaimsFromUserCommandRequest : BaseAuthHandlerRequest<UnassignAllClaimsFromUserCommandResponse>
{
    public required string UserId { get; init; }
}

public class UnassignAllClaimsFromUserCommandResponse : ApplicationResponse<UnassignAllClaimsFromUserCommandResponse>;