using Application.Dtos.Authentication;
using Base.Application.Dtos.Interfaces;

namespace Application.Dtos.Users;

public class UserAdminDto : IEntityDto
{
    public string Id { get; set; }
    
    public string UserName { get; set; }
    public string Email { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    public string? AvatarUrl { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public bool IsBlocked { get; set; }

    public ICollection<RoleDto> Roles { get; set; } = new List<RoleDto>();
    public ICollection<ClaimDto> Claims { get; set; } = new List<ClaimDto>();
}