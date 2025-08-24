using Base.Domain.Models.Interfaces;

namespace Domain.Models.Users;

public class NewUserEmailModel : IEntityModel
{
    public string NewEmail { get; set; }
}