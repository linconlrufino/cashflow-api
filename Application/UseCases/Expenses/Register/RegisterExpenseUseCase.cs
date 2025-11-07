using Communication.Requests;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Repositories.Expenses;
using Exception.ExceptionsBase;

namespace Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesRepository expensesRepository;
    private readonly IUnitOfWork unitOfWork;

    public RegisterExpenseUseCase(IExpensesRepository expensesRepository, IUnitOfWork unitOfWork)
    {
        this.expensesRepository = expensesRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<RegisterExpenseRequest> Execute(RegisterExpenseRequest request)
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

        await expensesRepository.AddAsync(entity);
        await unitOfWork.Commit();
        
        return new RegisterExpenseRequest();
    }

    private static void Validate(RegisterExpenseRequest request)
    {
        var validator = new RegisterExpenseValidator();
        var result = validator.Validate(request);

        if (result.IsValid)
            return;
        
        var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}