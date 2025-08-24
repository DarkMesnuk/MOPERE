using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Base.Infrastructure.Database.PostgreSQL.Configs;

public static class PostgreSQLConfig
{
    public static string GetConnectionString(IConfiguration configuration, string subNameSection = "")
    {
        var config = configuration.GetSection(subNameSection + PostgreSQLConfigurations.ConfigSectionName).Get<PostgreSQLConfigurations>()!;
        
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = config.Host,
            Port = config.Port,
            Database = config.Name,
            Username = config.User,
            Password = config.Password,
            CommandTimeout = config.CommandTimeout
        };

        return builder.ToString();
    }
}