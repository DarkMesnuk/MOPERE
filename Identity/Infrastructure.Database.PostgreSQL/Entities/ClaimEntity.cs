using Base.Domain.Entities.Interfaces;

namespace Infrastructure.Database.PostgreSQL.Entities;

public class ClaimEntity : IEntity<string>, IEntityDateTime
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    public ICollection<RoleClaimEntity>? Roles { get; set; }
    public ICollection<UserClaimEntity>? Users { get; set; }
}