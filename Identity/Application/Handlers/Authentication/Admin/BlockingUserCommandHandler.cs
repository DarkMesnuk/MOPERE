using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Authentication.Admin;

public class BlockingUserCommandHandler(
    ILogger<BlockingUserCommandHandler> logger,
    IUsersRepository usersRepository
) : BaseHandler<BlockingUserCommandHandler, BlockingUserCommandRequest, BlockingUserCommandResponse>(logger)
{
    public override async Task<BlockingUserCommandResponse> Handle(BlockingUserCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new BlockingUserCommandResponse();
        
        var user = await usersRepository.GetByIdAsync(request.UserId);
        
        user.IsBlocked = request.IsBlocked;
        
        await usersRepository.UpdateAsync(user);

        return response;
    }
}

public class BlockingUserCommandRequest : BaseAuthHandlerRequest<BlockingUserCommandResponse>
{
    public required string UserId { get; init; }
    public required bool IsBlocked { get; init; }
}

public class BlockingUserCommandResponse : ApplicationResponse<BlockingUserCommandResponse>;