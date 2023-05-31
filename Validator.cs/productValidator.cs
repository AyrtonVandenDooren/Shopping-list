namespace Shops.Validator;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("De naam is verplicht").MinimumLength(5).WithMessage("Minstens 5 karakters");
        RuleFor(c => c.category).NotEmpty().WithMessage("Je hebt geen category");
    }
}
