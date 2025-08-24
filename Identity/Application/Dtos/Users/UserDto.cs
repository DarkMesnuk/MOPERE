using Base.Application.Dtos.Interfaces;

namespace Application.Dtos.Users;

public class UserDto : IEntityDto
{
    public string Id { get; set; }
    
    public string UserName { get; set; }
    public string Email { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    public string? AvatarUrl { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}