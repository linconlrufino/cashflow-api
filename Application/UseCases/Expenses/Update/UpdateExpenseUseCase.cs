using AutoMapper;
using Communication.Requests;
using Communication.Requests.Expense;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Expenses;
using Domain.Services.LoggedUser;
using Exception;
using Exception.ExceptionsBase;

namespace Application.UseCases.Expenses.Update;

public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;
    private readonly IExpensesUpdateOnlyRepository repository;
    private readonly ILoggedUser loggedUser;

    public UpdateExpenseUseCase(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IExpensesUpdateOnlyRepository repository,
        ILoggedUser loggedUser)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
        this.repository = repository;
        this.loggedUser = loggedUser;
    }

    public async Task Execute(long id, ExpenseRequest request)
    {
        Validate(request);
        
        var loggedUserInfo = await loggedUser.Get();
        
        var expense = await repository.GetByIdAsync(loggedUserInfo.Id, id);
        
        if (expense is null )
            throw new NotFoundException(ResourcesErrorMessages.EXPENSE_NOT_FOUND);
        
        mapper.Map(request, expense);
        
        repository.Update(expense);
        await unitOfWork.Commit();
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