using Domain.Repositories;
using Domain.Repositories.Expenses;
using Domain.Services.LoggedUser;
using Exception;
using Exception.ExceptionsBase;

namespace Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository repository;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILoggedUser loggedUser;

    public DeleteExpenseUseCase(
        IExpensesWriteOnlyRepository repository,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser)
    {
        this.repository = repository;
        this.unitOfWork = unitOfWork;
        this.loggedUser = loggedUser;
    }
    
    public async Task Execute(long id)
    {
        var loggedUserInfo = await loggedUser.Get();
        var result = await repository.DeleteAsync(loggedUserInfo.Id, id);
        if (result is false)
            throw new NotFoundException(ResourcesErrorMessages.EXPENSE_NOT_FOUND);
        await unitOfWork.Commit();
    }
}