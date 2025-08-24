using Base.Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Database.PostgreSQL.Entities;

public class UserClaimEntity : IdentityUserClaim<string>, IEntity<int>
{
    public virtual UserEntity? User { get; set; }
    
    public string ClaimId { get; set; }
    public virtual ClaimEntity? Claim { get; set; }
}