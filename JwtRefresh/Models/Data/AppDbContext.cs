using JwtRefresh.Models.AuthModels;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace JwtRefresh.Models.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().
                HasIndex(u => u.Email).
                IsUnique(true);

            modelBuilder.Entity<User>().
                HasIndex(u => u.Username).
                IsUnique(true);

            modelBuilder.Entity<Role>().
                HasIndex(r => r.RoleName).
                IsUnique(true);

            modelBuilder.Entity<Role>()
                .HasData(
                new Role {Id = Guid.NewGuid(), RoleName = "User" },
                new Role() {Id = Guid.NewGuid(), RoleName = "Admin" });
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<UserToken> UserTokens { get; set; } = null!;
    }
}