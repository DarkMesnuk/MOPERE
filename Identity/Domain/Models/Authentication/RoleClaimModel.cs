using Base.Domain.Models.Interfaces;

namespace Domain.Models.Authentication;

public class RoleClaimModel : IEntityModel<int> 
{
    public int Id { get; set; }
    
    public RoleModel? Role { get; set; }
    public ClaimModel? Claim { get; set; }
}