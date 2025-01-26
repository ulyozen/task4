using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vision360.Context.Models;

namespace Vision360.Context;

public class VisionDbContext : IdentityDbContext<User>
{
    public VisionDbContext(DbContextOptions<VisionDbContext> options) : base(options) {}
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .Property(u => u.Name)
            .HasMaxLength(256)
            .HasColumnType("varchar(256)");
        
        builder.Entity<User>()
            .HasIndex(u => u.NormalizedUserName)
            .IsUnique(false)
            .HasDatabaseName("UserNameIndex");
        
        builder.Entity<User>()
            .HasIndex(u => u.NormalizedEmail)
            .IsUnique()
            .HasDatabaseName("EmailIndex");
    }
}