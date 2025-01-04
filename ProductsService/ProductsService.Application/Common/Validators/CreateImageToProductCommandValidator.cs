using FluentValidation;
using Microsoft.AspNetCore.Http;
using ProductsService.Application.UseCases.ProductUseCases.Commands.CreateImage;

namespace ProductsService.Application.Common.Validators
{
    public class CreateImageToProductCommandValidator : AbstractValidator<CreateImageToProductCommand>
    {
        public CreateImageToProductCommandValidator()
        {
            RuleFor(command => command.File)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("File cannot be null")
                .Must(HaveValidImageFiles).WithMessage("Invalid image file.");
        }

        private bool HaveValidImageFiles(IFormFile file)
        {
            if (file == null)
                return false;

            if (file.Length > 5 * 1024 * 1024)
                return false;

            var acceptableTypes = new[] { "image/jpeg", "image/png", "image/gif" };

            if (!acceptableTypes.Contains(file.ContentType))
                return false;

            return true;
        }
    }
}
