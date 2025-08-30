using Communication.Requests;
using Exception;
using FluentValidation;

namespace Application.UseCases.Expenses.Register;

public class RegisterExpenseValidator : AbstractValidator<RegisterExpenseRequest>
{
    public RegisterExpenseValidator()
    {
        RuleFor(expense => expense.Title).NotEmpty().WithMessage(ResourcesErrorMessages.TITLE_REQUIRED);
        RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage(ResourcesErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
        RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourcesErrorMessages.EXPENSES_CANNOT_BE_FOR_THE_FUTURE);
        RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage(ResourcesErrorMessages.PAYMENT_TYPE_IS_NOT_VALID);
    }
}