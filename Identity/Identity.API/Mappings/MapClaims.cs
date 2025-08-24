using Identity.API.Requests.Claims;
using Application.Handlers.Claims;
using Base.API.Extensions;

namespace Identity.API.Mappings;

public partial class RequestsMappings
{
    private void CreateMapClaims()
    {
        this.CreateAuthMap<CreateClaimRequest, CreateClaimCommandRequest>();
        this.CreateAuthMap<GetClaimsRequest, GetClaimsQueryRequest>();
        this.CreateAuthMap<UpdateClaimRequest, UpdateClaimCommandRequest>();
        this.CreateAuthMap<DeleteClaimRequest, DeleteClaimCommandRequest>();

        this.CreateAuthMap<IsUserHaveClaimRequest, IsUserHaveClaimCommandRequest>();
        
        this.CreateAuthMap<SetClaimsToUserRequest, SetClaimsToUserCommandRequest>();
        this.CreateAuthMap<AssignClaimToUserRequest, AssignClaimToUserCommandRequest>();
        this.CreateAuthMap<UnassignClaimFromUserRequest, UnassignClaimFromUserCommandRequest>();
        this.CreateAuthMap<UnassignAllClaimsFromUserRequest, UnassignAllClaimsFromUserCommandRequest>();

        this.CreateAuthMap<SetClaimsToRoleRequest, SetClaimsToRoleCommandRequest>();
        this.CreateAuthMap<AssignClaimToRoleRequest, AssignClaimToRoleCommandRequest>();
        this.CreateAuthMap<UnassignClaimFromRoleRequest, UnassignClaimFromRoleCommandRequest>();
        this.CreateAuthMap<UnassignAllClaimsFromRoleRequest, UnassignAllClaimsFromRoleCommandRequest>();
    }
}