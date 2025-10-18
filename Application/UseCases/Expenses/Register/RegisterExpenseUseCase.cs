using Communication.Requests;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories.Expenses;
using Exception.ExceptionsBase;

namespace Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesRepository expensesRepository;

    public RegisterExpenseUseCase(IExpensesRepository expensesRepository)
    {
        this.expensesRepository = expensesRepository;
    }

    public RegisterExpenseRequest Execute(RegisterExpenseRequest request)
    {
        Validate(request);

        var entity = new Expense()
        {
            Title = request.Title,
            Description = request.Description,
            Date = request.Date,
            Amount = request.Amount,
            PaymentType = (PaymentType)request.PaymentType
        };

        expensesRepository.Add(entity);
        return new RegisterExpenseRequest();
    }

    private void Validate(RegisterExpenseRequest request)
    {
        var validator = new RegisterExpenseValidator();
        var result = validator.Validate(request);

        if (result.IsValid)
            return;
        
        var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}