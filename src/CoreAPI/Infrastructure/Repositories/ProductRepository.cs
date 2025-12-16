using CoreAPI.Domain.Entities;
using CoreAPI.Domain.Interfaces;
using CoreAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoreAPI.Infrastructure.Repositories;

/// <summary>
/// Implémentation concrète du repository avec EF Core
/// Pattern Repository pour l'accès aux données
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(
        ApplicationDbContext context,
        ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetByIdAsync(id, cancellationToken);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AnyAsync(p => p.Id == id, cancellationToken);
    }
}

