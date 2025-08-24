using Base.Domain.Chekers;
using Microsoft.Extensions.DependencyInjection;

namespace Service;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddService(this IServiceCollection services)
    { 
        services
            .AddTransient<IIdentityAccessChecker, IdentityAccessChecker>();
        
        return services;
    }
}