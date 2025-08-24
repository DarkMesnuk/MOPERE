using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Database.PostgreSQL;

internal class MopereIdentityFactory : IDesignTimeDbContextFactory<MopereIdentityContext>
{
    public MopereIdentityContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<MopereIdentityContext> optionsBuilder = new();

        optionsBuilder.UseNpgsql("User ID=postgres;Password=test;Host=localhost;Port=5432;Database=MopereIdentity;");
        
        return new MopereIdentityContext(optionsBuilder.Options);
    }
}