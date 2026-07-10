using BookingTemplate.Service.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingTemplate.Service.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Unique e-mail
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Default value configuration for CreatedAt and UpdatedAt property
        modelBuilder.Entity<User>()
            .Property(s => s.CreatedAtUtc)
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<User>()
            .Property(s => s.UpdatedAtUtc)
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Role>().HasData(
                    new Role { Id = 1, Name = "Admin" },
                    new Role { Id = 2, Name = "User" }
        );
    }
}
