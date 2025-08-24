using Identity.Interfaces;
using Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Identity;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services
            .AddScoped<IIdentityService, IdentityService>()
            .AddScoped<IRolesService, RolesService>();

        return services;
    }
}