using CoreAPI.Application.Commands;
using FluentValidation;

namespace CoreAPI.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour la commande CreateProductCommand
/// Validation des règles métier avant traitement
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom du produit est requis")
            .MaximumLength(200).WithMessage("Le nom ne peut pas dépasser 200 caractères");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("La description ne peut pas dépasser 1000 caractères");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Le prix doit être positif ou nul");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Le stock doit être positif ou nul");
    }
}

