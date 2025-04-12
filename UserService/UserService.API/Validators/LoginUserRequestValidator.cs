using FluentValidation;
using UserService.API.Dtos.Requests;

namespace UserService.API.Validators
{
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(l => l.Email)
                .EmailAddress().WithMessage("Incorrect email entered")
                .When(e => e != null);

            RuleFor(l => l.Password)
                 .Must(pass => pass.Length >= 8).WithMessage("Password length has to be at lest 8")
                 .When(p => p != null);
        }
    }
}
