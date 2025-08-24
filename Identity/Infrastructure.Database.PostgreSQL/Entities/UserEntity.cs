using Base.Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Database.PostgreSQL.Entities;

public class UserEntity : IdentityUser, IEntity<string>, IEntityDateTime, IEntityDeleted
{
    public string? VerificationCode { get; set; }
    public DateTime VerificationCodeLifeTime { get; set; }
    public int VerificationCountTries { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public string? AvatarUrl { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsBlocked { get; set; }

    public virtual ICollection<UserRoleEntity>? UserRoles { get; set; }
    public virtual ICollection<UserClaimEntity>? UserClaims { get; set; }
}