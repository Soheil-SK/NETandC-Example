using CoreAPI.Application.DTOs;

namespace WebApp.Services;

/// <summary>
/// Interface pour consommer la Core API
/// Abstraction pour l'appel HTTP vers l'API
/// </summary>
public interface ICoreApiService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task<Guid> CreateProductAsync(CreateProductRequest request);
    Task UpdateProductAsync(Guid id, UpdateProductRequest request);
    Task DeleteProductAsync(Guid id);
}

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public class UpdateProductRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
}

