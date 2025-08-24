using Base.Domain.Infrastructure.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Interfaces.Repositories.Users;
using Infrastructure.Database.PostgreSQL.Mapping;
using Infrastructure.Database.PostgreSQL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Database.PostgreSQL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MopereIdentityContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            });
            options.EnableSensitiveDataLogging();
        });
        
        services.AddAutoMapper(_ => { }, typeof(EntitiesMapping).Assembly);


        services.AddScoped<ITransactionManager, TransactionManager>();

        services
            .AddScoped<IUsersRepository, UsersRepository>()
            .AddScoped<IClaimsRepository, ClaimsRepository>()
            .AddScoped<IRolesRepository, RolesRepository>()
            .AddScoped<IRoleClaimsRepository, RoleClaimsRepository>()
            .AddScoped<IUserClaimsRepository, UserClaimsRepository>()
            .AddScoped<IUserRolesRepository, UserRolesRepository>()
            ;
        
        return services;
    }
}