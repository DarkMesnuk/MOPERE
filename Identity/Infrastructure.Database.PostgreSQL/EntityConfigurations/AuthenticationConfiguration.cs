using Base.Infrastructure.Database.PostgreSQL.Extensions;
using Infrastructure.Database.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.PostgreSQL.EntityConfigurations;

public static class AuthenticationConfiguration
{
    public static ModelBuilder ConfigurationAuthentication(this ModelBuilder builder)
    {
        builder
            .EntityValueGeneratedOnAdd<RoleEntity>()
            .EntityValueGeneratedOnAdd<ClaimEntity>()
            .EntityValueGeneratedOnAdd<RoleClaimEntity, int>()
            .EntityValueGeneratedOnAdd<UserClaimEntity, int>();
        
        builder.Entity<RoleEntity>()
            .HasMany(e => e.UserRoles)
            .WithOne(e => e.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<RoleEntity>()
            .HasMany(e => e.Claims)
            .WithOne(e => e.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<ClaimEntity>()
            .HasMany(e => e.Roles)
            .WithOne(e => e.Claim)
            .HasForeignKey(ur => ur.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<ClaimEntity>()
            .HasMany(e => e.Users)
            .WithOne(e => e.Claim)
            .HasForeignKey(ur => ur.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RoleClaimEntity>(entity =>
        {
            entity.HasIndex(e => new { e.RoleId, e.ClaimId })
                .IsUnique();
        });

        builder.Entity<UserClaimEntity>(entity =>
        {
            entity.HasIndex(e => new { e.UserId, e.ClaimId })
                .IsUnique();
        });
        
        return builder;
    }
}