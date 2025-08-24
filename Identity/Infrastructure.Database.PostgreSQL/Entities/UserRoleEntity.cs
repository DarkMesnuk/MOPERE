using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Database.PostgreSQL.Entities;

public class UserRoleEntity : IdentityUserRole<string>, IEntity<string>
{
    [NotMapped]
    public string Id { get; set; }
    public virtual UserEntity? User { get; set; }
    public virtual RoleEntity? Role { get; set; }
}
