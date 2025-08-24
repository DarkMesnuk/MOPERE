using Base.Domain.Entities.Interfaces;
using Infrastructure.Database.PostgreSQL.Entities;
using Infrastructure.Database.PostgreSQL.EntityConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.PostgreSQL;

public class MopereIdentityContext(
    DbContextOptions<MopereIdentityContext> options
) : IdentityDbContext<UserEntity, RoleEntity, string, UserClaimEntity, UserRoleEntity, IdentityUserLogin<string>, RoleClaimEntity, IdentityUserToken<string>>(options)
{
    public DbSet<ClaimEntity> Claims { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder
            .ConfigurationAuthentication()
            .ConfigurationUsers();
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is { Entity: IEntityDateTime, State: EntityState.Added or EntityState.Modified });

        foreach (var entityEntry in entries)
        {
            ((IEntityDateTime)entityEntry.Entity).ModifiedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
                ((IEntityDateTime)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
        }
        
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IEntityDateTime && (
                e.State == EntityState.Added ||
                e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((IEntityDateTime)entityEntry.Entity).ModifiedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
                ((IEntityDateTime)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }

}