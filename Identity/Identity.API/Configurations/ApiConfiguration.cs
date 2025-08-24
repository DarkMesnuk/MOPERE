using Identity.API.Mappings;
using Application;
using Base.Infrastructure.Database.PostgreSQL.Configs;
using Base.Infrastructure.Database.Redis.Configs;
using Infrastructure;
using Infrastructure.Database.PostgreSQL;
using Infrastructure.Database.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service;

namespace Identity.API.Configurations;

public static class ApiConfiguration
{
    public static IServiceCollection AddIdentityApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureConfigs(configuration);
        
        services.AddIdentity();
        
        services.AddApplication();
        services.AddIdentityServices();
        services.AddInfrastructure();
        
        var postgreSqlConnectionString = PostgreSQLConfig.GetConnectionString(configuration, "Identity-");
        services.AddPostgreInfrastructure(postgreSqlConnectionString);

        services.AddService();

        var redisEndpoint = RedisConfig.GetEndpointString(configuration);
        services.AddRedisInfrastructure(redisEndpoint);

        services.AddAutoMapper(_ => { }, typeof(RequestsMappings).Assembly);
        
        return services;
    }
}