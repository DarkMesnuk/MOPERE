using Base.Domain.Models.Interfaces;

namespace Domain.Models.Authentication;

public class RoleModel : IEntityModel<string>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    public ICollection<ClaimModel>? Claims { get; set; }
}