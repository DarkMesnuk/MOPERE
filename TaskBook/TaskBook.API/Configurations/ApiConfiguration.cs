using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskBook.API.Configurations;

public static class ApiConfiguration
{
    public static IServiceCollection AddTaskBookApi(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}