using FluentValidation;
using UserService.API.Dtos.Requests;

namespace UserService.API.Validators
{
    public class ActivateAccountRequestValidator : AbstractValidator<ActivateAccountRequest>
    {
        public ActivateAccountRequestValidator()
        {
            RuleFor(r => r.ActivateCode)
                .InclusiveBetween(100000, 999999).WithMessage("The activation code must be a 6-digit number.")
                .When(c => c != null);
        }
    }
}
