namespace Shops.Validator;

public class ShopValidator : AbstractValidator<Shop>
{
    public ShopValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("De naam is verplicht").MinimumLength(5).WithMessage("Minstens 5 karakters");
    }
}
