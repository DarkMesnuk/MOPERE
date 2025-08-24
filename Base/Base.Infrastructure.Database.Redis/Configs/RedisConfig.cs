using Microsoft.Extensions.Configuration;

namespace Base.Infrastructure.Database.Redis.Configs;

public static class RedisConfig
{
    public static string GetEndpointString(IConfiguration configuration)
    {
        var config = configuration.GetSection(RedisConfigurations.ConfigSectionName).Get<RedisConfigurations>()!;

        return config.Endpoint;
    }
}