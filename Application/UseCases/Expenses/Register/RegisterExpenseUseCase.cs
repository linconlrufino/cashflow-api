using AutoMapper;
using Communication.Requests;
using Communication.Requests.Expense;
using Communication.Responses;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Expenses;
using Domain.Services.LoggedUser;
using Exception.ExceptionsBase;

namespace Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository repository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly ILoggedUser loggedUser;

    public RegisterExpenseUseCase(
        IExpensesWriteOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        this.repository = repository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.loggedUser = loggedUser;
    }

    public async Task<RegisteredExpenseResponse> Execute(ExpenseRequest request)
    {
        Validate(request);

        var loggedUserInfo = await loggedUser.Get();
        
        var expense = mapper.Map<Expense>(request);
        expense.UserId = loggedUserInfo.Id;
        
        await repository.AddAsync(expense);
        await unitOfWork.Commit();
        
        return mapper.Map<RegisteredExpenseResponse>(expense);
    }

    private static void Validate(ExpenseRequest request)
    {
        var validator = new ExpenseValidator();
        var result = validator.Validate(request);

        if (result.IsValid)
            return;
        
        var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}