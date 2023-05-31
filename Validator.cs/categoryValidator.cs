namespace Shops.Validator;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("De naam is verplicht").MinimumLength(5).WithMessage("Minstens 5 karakters");
        RuleFor(c => c.shop).NotEmpty().WithMessage("Je hebt geen shops");
    }
}
