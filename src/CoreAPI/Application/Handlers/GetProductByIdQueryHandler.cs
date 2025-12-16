using AutoMapper;
using CoreAPI.Application.DTOs;
using CoreAPI.Application.Queries;
using CoreAPI.Domain.Interfaces;
using MediatR;

namespace CoreAPI.Application.Handlers;

/// <summary>
/// Handler MediatR pour traiter la query GetProductByIdQuery
/// Pattern Query du CQRS - lecture seule
/// </summary>
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    public GetProductByIdQueryHandler(
        IProductRepository repository,
        IMapper mapper,
        ILogger<GetProductByIdQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération du produit: {ProductId}", request.Id);

        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (product == null)
        {
            _logger.LogWarning("Produit non trouvé: {ProductId}", request.Id);
            return null;
        }

        // Mapping automatique via AutoMapper
        return _mapper.Map<ProductDto>(product);
    }
}

