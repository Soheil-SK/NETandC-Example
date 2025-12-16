namespace CoreAPI.Domain.Entities;

/// <summary>
/// Entité Produit représentant le domaine métier
/// Exemple concret d'entité avec règles métier
/// </summary>
public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Constructeur privé pour forcer l'utilisation de la factory
    private Product() { }

    // Factory method pour créer un produit avec validation
    public static Product Create(string name, string description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Le nom du produit est requis", nameof(name));
        
        if (price < 0)
            throw new ArgumentException("Le prix ne peut pas être négatif", nameof(price));
        
        if (stock < 0)
            throw new ArgumentException("Le stock ne peut pas être négatif", nameof(stock));

        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description ?? string.Empty,
            Price = price,
            Stock = stock,
            CreatedAt = DateTime.UtcNow
        };
    }

    // Méthodes métier
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("Le prix ne peut pas être négatif", nameof(newPrice));
        
        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStock(int newStock)
    {
        if (newStock < 0)
            throw new ArgumentException("Le stock ne peut pas être négatif", nameof(newStock));
        
        Stock = newStock;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsInStock() => Stock > 0;

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La quantité doit être positive", nameof(quantity));
        
        if (Stock < quantity)
            throw new InvalidOperationException("Stock insuffisant");

        Stock -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Le nom du produit est requis", nameof(newName));
        
        Name = newName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }
}

