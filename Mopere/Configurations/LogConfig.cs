using Serilog;

namespace Mopere.Configurations;

public static class LogConfig
{
    public static void UseSerilog(this WebApplicationBuilder builder, IConfigurationRoot config)
    {
        var loggingConfig = config.GetSection("Serilog");
        loggingConfig["LogGroup"] = $"dev/{loggingConfig["LogGroup"]}";
        
        builder.Host.UseSerilog((_, loggerConfig) => {
            loggerConfig
                .WriteTo.Console().ReadFrom.Configuration(config)
                .ReadFrom.Configuration(config);
        });
    }
}