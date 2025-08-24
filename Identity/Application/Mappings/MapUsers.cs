using Application.Dtos.Users;
using Domain.Models.Authentication;
using Domain.Models.Users;

namespace Application.Mappings;

public partial class DtosMappings
{
    private void CreateMapUsers()
    {
        CreateMap<UserModel, UserDto>();
        CreateMap<UserModel, UserAdminDto>()
            .ForMember(x => x.Roles, expression => expression.MapFrom(model => model.UserRoles != null ? model.UserRoles.Select(ur => ur.Role) : new List<RoleModel>()))
            .ForMember(x => x.Claims, expression => expression.MapFrom(model => model.UserClaims != null ? model.UserClaims.Select(ur => ur.Claim) : new List<ClaimModel>()));
    }
}