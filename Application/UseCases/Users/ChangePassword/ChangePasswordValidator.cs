using Communication.Requests.User;
using FluentValidation;

namespace Application.UseCases.Users.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator<ChangePasswordRequest>());
    }
}