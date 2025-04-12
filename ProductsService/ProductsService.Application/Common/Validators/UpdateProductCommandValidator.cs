using FluentValidation;
using ProductsService.Application.UseCases.ProductUseCases.Commands.Update;

namespace ProductsService.Application.Common.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Name)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage("Name is required.")
               .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters.");

            RuleFor(command => command.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot be longer than 500 characters.");

            RuleFor(command => command.Quantity)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(command => command.Price)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
}
