using AutoMapper;

namespace Application.Mappings;

public partial class DtosMappings : Profile
{
    public DtosMappings()
    {
        CreateMapAuthentication();
        CreateMapUsers();
    }
}