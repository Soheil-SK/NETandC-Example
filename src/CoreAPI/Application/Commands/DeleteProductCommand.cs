using MediatR;

namespace CoreAPI.Application.Commands;

/// <summary>
/// Commande CQRS pour supprimer un produit
/// </summary>
public class DeleteProductCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

