using Communication.Requests;
using FluentValidation;

namespace Application.UseCases.Expenses.Register;

public class RegisterExpenseValidator : AbstractValidator<RegisterExpenseRequest>
{
    public RegisterExpenseValidator()
    {
        RuleFor(expense => expense.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Expenses cannot be for the future.");
        RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage("Payment type is not valid.");
    }
}