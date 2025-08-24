using Base.Domain.Models.Interfaces;

namespace Domain.Models.Authentication;

public class TokenModel : IEntityModel
{
    public string AccessToken { get; set; }
    public DateTime AccessTokenLifeTime { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenLifeTime { get; set; }
}