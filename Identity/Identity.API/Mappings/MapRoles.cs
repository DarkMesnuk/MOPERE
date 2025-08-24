using Identity.API.Requests.Roles;
using Application.Handlers.Roles;
using Base.API.Extensions;

namespace Identity.API.Mappings;

public partial class RequestsMappings
{
    private void CreateMapRoles()
    {
        this.CreateAuthMap<CreateRoleRequest, CreateRoleCommandRequest>();
        this.CreateAuthMap<UpdateRoleRequest, UpdateRoleCommandRequest>();
        this.CreateAuthMap<DeleteRoleRequest, DeleteRoleCommandRequest>();
        
        this.CreateAuthMap<GetRolesRequest, GetRolesCommandRequest>();

        this.CreateAuthMap<AssignRoleToUserRequest, AssignRoleToUserCommandRequest>();
        this.CreateAuthMap<UnassignRoleFromUserRequest, UnassignRoleFromUserCommandRequest>();
        this.CreateAuthMap<UnassignAllRolesFromUserRequest, UnassignAllRolesFromUserCommandRequest>();
    }
}