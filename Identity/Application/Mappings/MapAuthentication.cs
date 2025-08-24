using Application.Dtos.Authentication;
using Domain.Models.Authentication;

namespace Application.Mappings;

public partial class DtosMappings
{
    private void CreateMapAuthentication()
    {
        CreateMap<TokenModel, TokenDto>();
        
        CreateMap<ClaimModel, ClaimDto>();
        CreateMap<RoleModel, RoleDto>();
    }
}