using Identity.API.Requests.Users;
using Identity.API.Requests.Users.Admin;
using Application.Handlers.Users;
using Application.Handlers.Users.Admin;
using Base.API.Extensions;

namespace Identity.API.Mappings;

public partial class RequestsMappings
{
    private void CreateMapUsers()
    {
        this.CreateAuthMap<CreateAdminUserRequest, CreateAdminUserCommandRequest>();
        this.CreateAuthMap<GetUsersAdminRequest, GetUsersAdminQueryRequest>();
        this.CreateAuthMap<GetUserByIdAdminRequest, GetUserByIdAdminQueryRequest>();
        this.CreateAuthMap<UpdateUserByIdRequest, UpdateUserByIdCommandRequest>();
        this.CreateAuthMap<DeleteUserByIdRequest, DeleteUserByIdCommandRequest>();
        
        this.CreateAuthMap<GetUserRequest, GetUserQueryRequest>();
        this.CreateAuthMap<GetUsersRequest, GetUsersQueryRequest>();
        this.CreateAuthMap<UpdateUserRequest, UpdateUserCommandRequest>();
        this.CreateAuthMap<DeleteUserRequest, DeleteUserCommandRequest>();
        this.CreateAuthMap<ConfirmDeleteUserRequest, ConfirmDeleteUserCommandRequest>();
    }
}