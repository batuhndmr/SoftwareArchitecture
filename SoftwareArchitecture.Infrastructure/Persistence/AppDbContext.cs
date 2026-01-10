using Microsoft.EntityFrameworkCore;
using SoftwareArchitecture.Domain.Entities;

namespace SoftwareArchitecture.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User entity yapılandırması
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(500);
                
                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("User");
            });

            // User -> Orders (1-N)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order -> OrderItems (1-N)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product -> OrderItems (1-N)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // BaseEntity'den türeyen entity'ler için
            var baseEntityEntries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in baseEntityEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            // OrderItem için (BaseEntity'den türemiyor)
            var orderItemEntries = ChangeTracker.Entries<OrderItem>();
            foreach (var entry in orderItemEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            // Product için (BaseEntity'den türemiyor)
            var productEntries = ChangeTracker.Entries<Product>();
            foreach (var entry in productEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            // Order için (BaseEntity'den türemiyor)
            var orderEntries = ChangeTracker.Entries<Order>();
            foreach (var entry in orderEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        
    }
}
