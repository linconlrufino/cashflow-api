using Domain.Entities;

namespace Domain.Repositories.Expenses;

public interface IExpensesUpdateOnlyRepository
{
    Task<Expense?> GetByIdAsync(long id);
    void Update(Expense expense);
}