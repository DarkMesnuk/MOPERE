using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Users;
using Domain.Schemas.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Users;

public class UpdateUserCommandHandler(
    ILogger<UpdateUserCommandHandler> logger,
    IUsersRepository usersRepository
) : BaseHandler<UpdateUserCommandHandler, UpdateUserCommandRequest, UpdateUserCommandResponse>(logger)
{
    public override async Task<UpdateUserCommandResponse> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new UpdateUserCommandResponse();
            
        var user = await usersRepository.GetByIdAsync(request.AuthUserId);
        
        await usersRepository.UpdateBySchemaAsync(user, request);

        return response;
    }
}

public class UpdateUserCommandRequest : BaseAuthHandlerRequest<UpdateUserCommandResponse>, IUpdateUserSchema
{
    public string? UserName { get; init; }
    
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    
    public bool? DeleteAvatar { get; init; }
}

public class UpdateUserCommandResponse : ApplicationResponse<UpdateUserCommandResponse>;