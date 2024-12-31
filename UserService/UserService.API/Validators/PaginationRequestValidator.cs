using FluentValidation;
using UserService.API.Dtos.Requests;

namespace UserService.API.Validators
{
    public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
    {
        public PaginationRequestValidator()
        {
            RuleFor(pr => pr.PageSize)
                .GreaterThan(0);

            RuleFor(pr => pr.Page)
                .GreaterThan(0);
        }
    }
}
