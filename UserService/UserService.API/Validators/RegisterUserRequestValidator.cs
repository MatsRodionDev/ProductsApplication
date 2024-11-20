using FluentValidation;
using UserService.API.Dtos.Requests;

namespace UserService.API.Validators
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(r => r.FirstName)
                .MinimumLength(4).WithMessage("First Name length has to be at lesat 4")
                .When(fn => fn != null);

            RuleFor(r => r.LastName)
                .MinimumLength(4).WithMessage("Last Name length has to be at lesat 4")
                .When(ln => ln != null);

            RuleFor(l => l.Email)
                .EmailAddress().WithMessage("Incorrect email entered")
                .When(e => e != null);

            RuleFor(l => l.Password)
                 .MinimumLength(8).WithMessage("Password length has to be at lest 8")
                 .When(p => p != null);
        }
    }
}
