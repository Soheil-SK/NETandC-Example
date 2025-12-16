using MediatR;

namespace CoreAPI.Application.Commands;

/// <summary>
/// Commande CQRS pour mettre à jour un produit
/// </summary>
public class UpdateProductCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
}

