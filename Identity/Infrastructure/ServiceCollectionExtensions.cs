using Domain.Interfaces.Infrastructure;
using Domain.Interfaces.Infrastructure.Storages;
using Infrastructure.Data;
using Infrastructure.Emails;
using Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddTransient<SeedAdminAccountData>();
        
        services
            .AddScoped<IEmailService, EmailService>();
        
        services
            .AddScoped<IAvatarsStorage, AvatarsStorage>();
            
        return services;
    }
}