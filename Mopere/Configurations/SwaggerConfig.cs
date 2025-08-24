using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Mopere.Configurations;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options => {
            options.SwaggerDoc("v1", new OpenApiInfo {
                Title = "Mopere API",
                Version = "v0.0.1"
            });

            var jwtSecurityScheme = new OpenApiSecurityScheme {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Put JWT Bearer token",
            
                Reference = new OpenApiReference {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            
            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
    
            // var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
            //
            // options.IncludeXmlComments(filePath);
        });

        return services;
    }

    public static void UseMopereSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Mopere API");
        });
    }
}