using AutoMapper;

namespace Identity.API.Mappings;

public partial class RequestsMappings : Profile
{
    public RequestsMappings()
    {
        CreateMapAuthentication();
        CreateMapClaims();
        CreateMapRoles();
        CreateMapUsers();
    }
}