using Base.Infrastructure.Database.PostgreSQL.Configs;
using Base.Infrastructure.Storage.Configs;
using Identity.Configs;
using Infrastructure.Data.Configs;
using Infrastructure.Emails.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.API.Configurations;

public static class SetUpConfig
{
    public static void ConfigureConfigs(this IServiceCollection service, IConfiguration config)
    {
        service
            .Configure<JwtConfigurations>(config.GetSection(JwtConfigurations.ConfigSectionName))
            .Configure<StorageConfigurations>(config.GetSection(StorageConfigurations.ConfigSectionName))
            .Configure<EmailConfiguration>(config.GetSection(EmailConfiguration.ConfigSectionName))
            .Configure<PostgreSQLConfigurations>(config.GetSection("Identity-" + PostgreSQLConfigurations.ConfigSectionName))
            .Configure<AdminCredentialsConfigurations>(config.GetSection(AdminCredentialsConfigurations.ConfigSectionName));
    }
}