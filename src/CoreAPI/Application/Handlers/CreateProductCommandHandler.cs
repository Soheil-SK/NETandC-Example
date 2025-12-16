using AutoMapper;
using CoreAPI.Application.Commands;
using CoreAPI.Application.DTOs;
using CoreAPI.Domain.Entities;
using CoreAPI.Domain.Interfaces;
using MediatR;

namespace CoreAPI.Application.Handlers;

/// <summary>
/// Handler MediatR pour traiter la commande CreateProductCommand
/// Implémentation du pattern CQRS avec séparation Commands/Queries
/// </summary>
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _repository;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(
        IProductRepository repository,
        ILogger<CreateProductCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Création d'un nouveau produit: {ProductName}", request.Name);

        // Création de l'entité via la factory method
        var product = Product.Create(
            request.Name,
            request.Description,
            request.Price,
            request.Stock);

        // Sauvegarde via le repository
        var createdProduct = await _repository.AddAsync(product, cancellationToken);

        _logger.LogInformation("Produit créé avec succès: {ProductId}", createdProduct.Id);

        return createdProduct.Id;
    }
}

