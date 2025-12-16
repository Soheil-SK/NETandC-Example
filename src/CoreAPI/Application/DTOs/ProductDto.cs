namespace CoreAPI.Application.DTOs;

/// <summary>
/// DTO (Data Transfer Object) pour exposer les données via l'API
/// Séparation entre le domaine et la couche présentation
/// </summary>
public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsInStock { get; set; }
}

