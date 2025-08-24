using Base.Domain.Models.Interfaces;
using Domain.Models.Users;

namespace Domain.Models.Authentication;

public class UserClaimModel : IEntityModel<int>
{
    public int Id { get; set; }
    
    public UserModel User { get; set; }
    public ClaimModel Claim { get; set; }
}