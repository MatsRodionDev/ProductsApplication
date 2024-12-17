using FluentValidation;
using OrderService.Application.UseCases.OrderUseCases.Create;

namespace OrderService.Application.Common.Validators
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(r => r.Quantity)
               .GreaterThan(0).WithMessage("Quantity has to ba greater than 0");
        }
    }
}
