using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.API.Configurations;

public static class ApiConfiguration
{
    public static IServiceCollection AddMessengerApi(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}