using Base.Domain.Models.Interfaces;
using Domain.Models.Users;

namespace Domain.Models.Authentication;

public class ClaimModel : IEntityModel<string>
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    public ICollection<RoleModel>? Roles { get; set; }
    public ICollection<UserModel>? Users { get; set; }
}