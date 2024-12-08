using FluentValidation;
using Microsoft.AspNetCore.Http;
using ProductsService.Application.UseCases.ProductUseCases.Commands.Create;

namespace ProductsService.Application.Common.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
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

            RuleFor(command => command.UserId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(command => command.ImageFiles)
                .Cascade(CascadeMode.Stop)
                .Must(HaveValidImageFiles).WithMessage("Invalid image files.");
        }

        private bool HaveValidImageFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return true;

            foreach (var file in files)
            {
                if (file.Length > 5 * 1024 * 1024) 
                    return false;

                var acceptableTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                if (!acceptableTypes.Contains(file.ContentType))
                    return false;
            }

            return true;
        }
    }
}
