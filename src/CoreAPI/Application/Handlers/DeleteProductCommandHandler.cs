using CoreAPI.Application.Commands;
using CoreAPI.Domain.Interfaces;
using MediatR;

namespace CoreAPI.Application.Handlers;

/// <summary>
/// Handler pour supprimer un produit
/// </summary>
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _repository;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(
        IProductRepository repository,
        ILogger<DeleteProductCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Suppression du produit: {ProductId}", request.Id);

        var exists = await _repository.ExistsAsync(request.Id, cancellationToken);

        if (!exists)
        {
            _logger.LogWarning("Produit non trouvé: {ProductId}", request.Id);
            throw new KeyNotFoundException($"Produit avec l'ID {request.Id} non trouvé");
        }

        await _repository.DeleteAsync(request.Id, cancellationToken);

        _logger.LogInformation("Produit supprimé avec succès: {ProductId}", request.Id);

        return Unit.Value;
    }
}

