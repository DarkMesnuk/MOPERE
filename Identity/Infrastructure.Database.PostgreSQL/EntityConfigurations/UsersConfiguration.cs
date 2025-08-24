using Base.Infrastructure.Database.PostgreSQL.Extensions;
using Infrastructure.Database.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.PostgreSQL.EntityConfigurations;

public static class UsersConfiguration
{
    public static ModelBuilder ConfigurationUsers(this ModelBuilder builder)
    {
        builder
            .EntityValueGeneratedOnAdd<UserEntity>();
        
        builder.Entity<UserEntity>()
            .HasMany(e => e.UserRoles)
            .WithOne(e => e.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<UserEntity>()
            .HasMany(e => e.UserClaims)
            .WithOne(e => e.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        return builder;
    }
}