using Domain.Entities;

namespace Domain.Repositories.Expenses;

public interface IExpensesReadOnlyRepository
{
    Task<IEnumerable<Expense>> GetAllAsync();
    Task<Expense?> GetByIdAsync(long id);
}