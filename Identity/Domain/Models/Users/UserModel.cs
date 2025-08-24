using Domain.Models.Authentication;
using Base.Domain.Models.Interfaces;

namespace Domain.Models.Users;

public class UserModel : IEntityModel<string>
{
    public string Id { get; set; }
    
    public string UserName { get; set; }
    public string Email { get; set; }
    
    public string? VerificationCode { get; set; }
    public DateTime VerificationCodeLifeTime { get; set; }
    public int VerificationCountTries { get; set; }

    #region Identity Properties
    public string? SecurityStamp { get; set; }
    public string? ConcurrencyStamp { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool LockoutEnabled { get; set; }
    public int AccessFailedCount { get; set; }
    public string? PasswordHash { get; set; }
    #endregion
    
    public bool EmailConfirmed { get; set; }
    
    public DateTimeOffset? LockoutEnd { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public string? AvatarUrl { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    
    public bool IsBlocked { get; set; }

    public ICollection<UserRoleModel>? UserRoles { get; set; }
    public ICollection<UserClaimModel>? UserClaims { get; set; }
}