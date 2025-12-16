using CoreAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreAPI.Infrastructure.Data;

/// <summary>
/// DbContext EF Core pour l'accès à la base de données
/// Configuration de la couche Infrastructure
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration de l'entité Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Stock).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}

