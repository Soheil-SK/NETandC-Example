using CoreAPI.Application.DTOs;
using MediatR;

namespace CoreAPI.Application.Queries;

/// <summary>
/// Query CQRS pour récupérer tous les produits
/// </summary>
public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
{
}

