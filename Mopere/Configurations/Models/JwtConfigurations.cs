namespace Mopere.Configurations.Models;

public class JwtConfigurations
{
    public const string ConfigSectionName = "Jwt";

    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public string RefreshKey { get; set; }
    public int LifeTimeSeconds { get; set; }
    public int RefreshLifeTimeSeconds { get; set; }
}