using AutoMapper;
using Communication.Responses;
using Domain.Repositories.Expenses;
using Domain.Services.LoggedUser;
using Exception;
using Exception.ExceptionsBase;

namespace Application.UseCases.Expenses.GetById;

public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpensesReadOnlyRepository repository;
    private readonly IMapper mapper;
    private readonly ILoggedUser loggedUser;

    public GetExpenseByIdUseCase(
        IExpensesReadOnlyRepository repository,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.loggedUser = loggedUser;
    }
    
    public async Task<ExpenseResponse> Execute(long id)
    {
        var loggedUserInfo = await loggedUser.Get();
        var result = await repository.GetByIdAsync(loggedUserInfo.Id, id);
        if (result is null)
            throw new NotFoundException(ResourcesErrorMessages.EXPENSE_NOT_FOUND);
        
        return mapper.Map<ExpenseResponse>(result);
    }
}