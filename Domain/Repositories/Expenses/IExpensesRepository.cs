using Domain.Entities;

namespace Domain.Repositories.Expenses;

public interface IExpensesRepository
{
    void Add(Expense expense);
}