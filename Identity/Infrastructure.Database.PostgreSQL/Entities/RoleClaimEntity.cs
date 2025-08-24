using Base.Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Database.PostgreSQL.Entities;

public class RoleClaimEntity : IdentityRoleClaim<string>, IEntity<int>
{
    public virtual RoleEntity? Role { get; set; }
    
    public string ClaimId { get; set; }
    public virtual ClaimEntity? Claim { get; set; }
}