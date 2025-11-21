using Domain.Entities;

namespace Domain.Repositories.Expenses;

public interface IExpensesWriteOnlyRepository
{
    Task AddAsync(Expense expense);
    Task<bool> DeleteAsync(long id);
}