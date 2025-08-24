using Base.Application.Dtos.Interfaces;

namespace Application.Dtos.Authentication;

public class TokenDto : IEntityDto
{
    public string AccessToken { get; set; }
    public DateTime AccessTokenLifeTime { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenLifeTime { get; set; }
}