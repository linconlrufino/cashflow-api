using Domain.Entities;

namespace Domain.Repositories.Expenses;

public interface IExpensesRepository
{ 
    Task AddAsync(Expense expense);
}