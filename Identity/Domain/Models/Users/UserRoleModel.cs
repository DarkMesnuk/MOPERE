using Base.Domain.Models.Interfaces;
using Domain.Models.Authentication;

namespace Domain.Models.Users;

public class UserRoleModel : IEntityModel<string>
{
    public string Id { get; set; }
    public UserModel User { get; set; }
    public RoleModel Role { get; set; }
}