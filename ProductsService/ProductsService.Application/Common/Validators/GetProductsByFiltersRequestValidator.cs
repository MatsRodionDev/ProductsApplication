using FluentValidation;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetByFilters;

namespace ProductsService.Application.Common.Validators
{
    public class GetProductsByFiltersRequestValidator : AbstractValidator<GetProductsByFiltersRequest>
    {
        public GetProductsByFiltersRequestValidator()
        {
            RuleFor(r => r.Page)
                .GreaterThan(0).WithMessage("Page value has to be at least 1");
            RuleFor(r => r.PageSize)
                .GreaterThan(0).WithMessage("Page value has to be at least 1");
        }
    }
}
