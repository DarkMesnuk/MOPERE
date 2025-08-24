namespace Mopere.Configurations;

public static class CorsConfig
{
    public static void AddMopereCors(this WebApplicationBuilder builder)
    {
        var defaultHostsAllowed = builder.Configuration.GetSection("DefaultOrigins")?
            .GetChildren()?
            .Select(x => x.Value!)?
            .ToArray();

        builder.Services.AddCors(options => {
            options.AddDefaultPolicy(
                policyBuilder => {
                    policyBuilder.WithOrigins(defaultHostsAllowed ?? Array.Empty<string>())
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS");
                }
            );
        });
    }
}