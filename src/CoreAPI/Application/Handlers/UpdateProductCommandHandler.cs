using CoreAPI.Application.Commands;
using CoreAPI.Domain.Interfaces;
using MediatR;

namespace CoreAPI.Application.Handlers;

/// <summary>
/// Handler pour mettre à jour un produit
/// </summary>
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductRepository _repository;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(
        IProductRepository repository,
        ILogger<UpdateProductCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Mise à jour du produit: {ProductId}", request.Id);

        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (product == null)
        {
            _logger.LogWarning("Produit non trouvé: {ProductId}", request.Id);
            throw new KeyNotFoundException($"Produit avec l'ID {request.Id} non trouvé");
        }

        // Mise à jour via les méthodes métier de l'entité
        if (request.Name != null)
            product.UpdateName(request.Name);
        
        if (request.Description != null)
            product.UpdateDescription(request.Description);
        
        if (request.Price.HasValue)
            product.UpdatePrice(request.Price.Value);
        
        if (request.Stock.HasValue)
            product.UpdateStock(request.Stock.Value);

        await _repository.UpdateAsync(product, cancellationToken);

        _logger.LogInformation("Produit mis à jour avec succès: {ProductId}", request.Id);

        return Unit.Value;
    }
}

