using MediatR;

namespace CoreAPI.Application.Commands;

/// <summary>
/// Commande CQRS pour créer un produit
/// Pattern Command du CQRS via MediatR
/// </summary>
public class CreateProductCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

