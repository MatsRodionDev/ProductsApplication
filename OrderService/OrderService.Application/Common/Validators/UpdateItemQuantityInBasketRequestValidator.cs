using FluentValidation;
using OrderService.Application.UseCases.BasketUseCases.RemoveItem;

namespace OrderService.Application.Common.Validators
{
    public class UpdateItemQuantityInBasketRequestValidator : AbstractValidator<UpdateItemQuantityInBasketRequest>
    {
        public UpdateItemQuantityInBasketRequestValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0).WithMessage("Quantity has to ba greater than 0");
        }
    }
}
