using Communication.Requests.Expense;
using Exception;
using FluentValidation;

namespace Application.UseCases.Users;

public class UserValidator : AbstractValidator<RegisterUserRequest>
{
    public UserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(ResourcesErrorMessages.NAME_EMPTY);
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ResourcesErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .WithMessage(ResourcesErrorMessages.EMAIL_INVALID);

        RuleFor(x => x.Password).SetValidator(new PasswordValidator<RegisterUserRequest>());
    }
}