using AutoMapper;
using CoreAPI.Application.DTOs;
using CoreAPI.Application.Queries;
using CoreAPI.Domain.Interfaces;
using MediatR;

namespace CoreAPI.Application.Handlers;

/// <summary>
/// Handler MediatR pour récupérer tous les produits
/// </summary>
public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductsQueryHandler> _logger;

    public GetAllProductsQueryHandler(
        IProductRepository repository,
        IMapper mapper,
        ILogger<GetAllProductsQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération de tous les produits");

        var products = await _repository.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}

