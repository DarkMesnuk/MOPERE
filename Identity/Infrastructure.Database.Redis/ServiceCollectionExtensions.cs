using Domain.Interfaces.Repositories.Users;
using Infrastructure.Database.Redis.Repositories;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure.Database.Redis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedisInfrastructure(this IServiceCollection services, string endpoint)
    {
        var config = new ConfigurationOptions
        {
            EndPoints = { endpoint },
            AbortOnConnectFail = false,
            Ssl = false
        };

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(config));

        services
            .AddScoped<INewUserEmailsRepository, NewUserEmailsRepository>();
        
        return services;
    }
}