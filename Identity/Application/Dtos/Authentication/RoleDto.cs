using Base.Application.Dtos.Interfaces;

namespace Application.Dtos.Authentication;

public class RoleDto : IEntityDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<ClaimDto>? Claims { get; set; }
}