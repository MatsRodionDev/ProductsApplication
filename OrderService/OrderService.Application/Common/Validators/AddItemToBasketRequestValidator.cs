using FluentValidation;
using OrderService.Application.UseCases.BasketUseCases.AddItem;

namespace OrderService.Application.Common.Validators
{
    public class AddItemToBasketRequestValidator : AbstractValidator<AddItemToBasketRequest>
    {
        public AddItemToBasketRequestValidator()
        {
            RuleFor(r => r.Quantity)
               .GreaterThan(0).WithMessage("Quantity has to ba greater than 0");
        }
    }
}
