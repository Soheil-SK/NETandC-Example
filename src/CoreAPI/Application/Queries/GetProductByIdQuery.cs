using CoreAPI.Application.DTOs;
using MediatR;

namespace CoreAPI.Application.Queries;

/// <summary>
/// Query CQRS pour récupérer un produit par ID
/// Pattern Query du CQRS via MediatR
/// </summary>
public class GetProductByIdQuery : IRequest<ProductDto?>
{
    public Guid Id { get; set; }
}

