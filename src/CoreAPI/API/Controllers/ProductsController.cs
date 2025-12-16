using CoreAPI.Application.Commands;
using CoreAPI.Application.DTOs;
using CoreAPI.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreAPI.API.Controllers;

/// <summary>
/// Controller REST pour les produits
/// Point d'entrée HTTP de l'API
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Protection JWT
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IMediator mediator,
        ILogger<ProductsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// GET /api/products
    /// Récupère tous les produits
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts(CancellationToken cancellationToken)
    {
        var query = new GetAllProductsQuery();
        var products = await _mediator.Send(query, cancellationToken);
        return Ok(products);
    }

    /// <summary>
    /// GET /api/products/{id}
    /// Récupère un produit par ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery { Id = id };
        var product = await _mediator.Send(query, cancellationToken);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    /// <summary>
    /// POST /api/products
    /// Crée un nouveau produit
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateProduct(
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var productId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId);
    }

    /// <summary>
    /// PUT /api/products/{id}
    /// Met à jour un produit existant
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// DELETE /api/products/{id}
    /// Supprime un produit
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand { Id = id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}

