using Base.Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Database.PostgreSQL.Entities;

public class RoleEntity : IdentityRole<string>, IEntity<string>, IEntityDateTime
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    public virtual ICollection<RoleClaimEntity>? Claims { get; set; }

    public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
}