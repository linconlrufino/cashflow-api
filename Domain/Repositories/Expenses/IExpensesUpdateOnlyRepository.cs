using Domain.Entities;

namespace Domain.Repositories.Expenses;

public interface IExpensesUpdateOnlyRepository
{
    Task<Expense?> GetByIdAsync(long userId, long id);
    
    void Update(Expense expense);
}