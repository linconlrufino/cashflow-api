using Domain.Entities;

namespace Domain.Repositories.Expenses;

public interface IExpensesReadOnlyRepository
{
    Task<IEnumerable<Expense>> GetAllAsync(long userId);
    Task<Expense?> GetByIdAsync(long userId, long id);
    Task<IEnumerable<Expense>> FilterByMonth(long userId, DateOnly date);
}