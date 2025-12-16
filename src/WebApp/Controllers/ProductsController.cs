using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

/// <summary>
/// Controller MVC pour la gestion des produits
/// Consomme la Core API pour toutes les opérations métier
/// </summary>
[Authorize]
public class ProductsController : Controller
{
    private readonly ICoreApiService _apiService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        ICoreApiService apiService,
        ILogger<ProductsController> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
        try
        {
            var products = await _apiService.GetProductsAsync();
            return View(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération des produits");
            return View("Error");
        }
    }

    // GET: Products/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var product = await _apiService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Products/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var request = new CreateProductRequest
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock
            };

            await _apiService.CreateProductAsync(request);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création du produit");
            ModelState.AddModelError("", "Une erreur s'est produite lors de la création du produit");
            return View(model);
        }
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var product = await _apiService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var model = new EditProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };

        return View(model);
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditProductViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var request = new UpdateProductRequest
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock
            };

            await _apiService.UpdateProductAsync(id, request);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la mise à jour du produit");
            ModelState.AddModelError("", "Une erreur s'est produite lors de la mise à jour du produit");
            return View(model);
        }
    }

    // GET: Products/Delete/5
    public async Task<IActionResult> Delete(Guid id)
    {
        var product = await _apiService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _apiService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la suppression du produit");
            return View("Error");
        }
    }
}

