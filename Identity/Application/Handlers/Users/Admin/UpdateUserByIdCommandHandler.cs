using Base.Application;
using Base.Application.Requests;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Domain.Interfaces.Repositories.Users;
using Domain.Schemas.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Users.Admin;

public class UpdateUserByIdCommandHandler(
    ILogger<UpdateUserByIdCommandHandler> logger,
    IUsersRepository usersRepository
) : BaseHandler<UpdateUserByIdCommandHandler, UpdateUserByIdCommandRequest, UpdateUserByIdCommandResponse>(logger)
{
    public override async Task<UpdateUserByIdCommandResponse> Handle(UpdateUserByIdCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new UpdateUserByIdCommandResponse();
            
        var user = await usersRepository.GetByIdAsync(request.UserId);

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            if (await usersRepository.ExistsByEmailAsync(request.Email))
            {
                return response.SetData(StatusCodes.AlreadyExists, $"Email \'{request.Email}\' already exists");
            }
        
            user.Email = request.Email.Trim();
        }
        
        await usersRepository.UpdateBySchemaAsync(user, request);

        return response;
    }
}

public class UpdateUserByIdCommandRequest : BaseAuthHandlerRequest<UpdateUserByIdCommandResponse>, IUpdateUserSchema
{
    public string UserId { get; set; } // FromRoute
    
    public string? Email { get; init; }
    
    public string? UserName { get; init; }
    
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    
    public bool? DeleteAvatar { get; init; }
}

public class UpdateUserByIdCommandResponse : ApplicationResponse<UpdateUserByIdCommandResponse>;