using Communication.Requests;
using Communication.Requests.User;
using Exception;
using FluentValidation;

namespace Application.UseCases.Users.Update;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourcesErrorMessages.NAME_EMPTY);
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourcesErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(user => string.IsNullOrEmpty(user.Email) == false, ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourcesErrorMessages.EMAIL_INVALID);
    }
}