using FluentValidation;
using UserService.API.Dtos.Requests;

namespace UserService.API.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(r => r.FirstName)
                .MinimumLength(4).WithMessage("First Name length has to be at lesat 4")
                .When(fn => fn != null);

            RuleFor(r => r.LastName)
                .MinimumLength(4).WithMessage("Last Name length has to be at lesat 4")
                .When(ln => ln != null);
        }
    }
}
