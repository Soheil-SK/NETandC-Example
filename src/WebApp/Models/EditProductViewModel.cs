using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

/// <summary>
/// ViewModel pour l'édition d'un produit
/// </summary>
public class EditProductViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Le nom est requis")]
    [StringLength(200, ErrorMessage = "Le nom ne peut pas dépasser 200 caractères")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le prix est requis")]
    [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Le stock est requis")]
    [Range(0, int.MaxValue, ErrorMessage = "Le stock doit être positif")]
    public int Stock { get; set; }
}

