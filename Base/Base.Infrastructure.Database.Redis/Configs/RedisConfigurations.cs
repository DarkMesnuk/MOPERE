namespace Base.Infrastructure.Database.Redis.Configs;

public class RedisConfigurations
{
    public const string ConfigSectionName = "Redis";
    
    public string Endpoint { get; set; }
}