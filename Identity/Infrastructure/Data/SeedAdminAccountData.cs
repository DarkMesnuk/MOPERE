using Base.Domain.Exceptions;
using Contract.Consts;
using Identity.Interfaces;
using Infrastructure.Data.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public class SeedAdminAccountData(
    IServiceProvider serviceProvider
)
{
    public async Task SeedDataAsync(IConfiguration config)
    {
        using var scope = serviceProvider.CreateScope();
        
        var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();
        var adminCredentials = config.GetSection("AdminCredentials").Get<AdminCredentialsConfigurations>();

        var admin = await identityService.GetByEmailOrDefaultAsync(adminCredentials.Email);
        
        if (admin == null)
        {
            var result = await identityService.CreateAndAssignRoleAsync(adminCredentials.Email, adminCredentials.Password, RoleConsts.SuperAdmin);

            if (!result.Succeeded)
            {
                throw new SomethingWentWrongException("Failed to create admin account");
            }
            
            admin = await identityService.GetByEmailAsync(adminCredentials.Email);

            admin.FirstName = "Admin";
            admin.LastName = "Admin";

            await identityService.UpdateAsync(admin);

            result = await identityService.ConfirmEmailAsync(admin);

            if (!result.Succeeded)
            {
                throw new SomethingWentWrongException("Failed to create admin account, confirm email");
            }
        }
    }
}