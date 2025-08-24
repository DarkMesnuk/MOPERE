using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Users.Admin;

public class DeleteUserByIdCommandHandler(
    ILogger<DeleteUserByIdCommandHandler> logger,
    IUsersRepository usersRepository
) : BaseHandler<DeleteUserByIdCommandHandler, DeleteUserByIdCommandRequest, DeleteUserByIdCommandResponse>(logger)
{
    public override async Task<DeleteUserByIdCommandResponse> Handle(DeleteUserByIdCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new DeleteUserByIdCommandResponse();

        var user = await usersRepository.GetByIdAsync(request.UserId);
        
        user.UserName = $"UN-{user.Id}";
        user.Email = $"UN-{user.Id}@UN.UN";
        user.PasswordHash = default;
        user.IsDeleted = true;
        user.DeletedAt = user.DeletedAt ?? DateTime.UtcNow;
        
        await usersRepository.UpdateAsync(user);

        return response;
    }
}

public class DeleteUserByIdCommandRequest : BaseAuthHandlerRequest<DeleteUserByIdCommandResponse>
{
    public string UserId { get; set; } // FromRoute
}

public class DeleteUserByIdCommandResponse : ApplicationResponse<DeleteUserByIdCommandResponse>;