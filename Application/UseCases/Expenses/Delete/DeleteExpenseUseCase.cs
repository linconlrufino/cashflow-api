using Domain.Repositories;
using Domain.Repositories.Expenses;
using Exception;
using Exception.ExceptionsBase;

namespace Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository repository;
    private readonly IUnitOfWork unitOfWork;

    public DeleteExpenseUseCase(
        IExpensesWriteOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        this.repository = repository;
        this.unitOfWork = unitOfWork;
    }
    
    public async Task Execute(long id)
    {
        var result = await repository.DeleteAsync(id);
        if (result is false)
            throw new NotFoundException(ResourcesErrorMessages.EXPENSE_NOT_FOUND);
        await unitOfWork.Commit();
    }
}