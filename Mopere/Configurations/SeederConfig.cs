using Infrastructure.Data;

namespace Mopere.Configurations;

public static class SeederConfig
{
    public static async Task RunDataSeeder(this IServiceProvider serviceProvider, IConfiguration config)
    {
        var adminSettingsSeeder = serviceProvider.GetService<SeedAdminAccountData>();

        await adminSettingsSeeder!.SeedDataAsync(config);
    }
}