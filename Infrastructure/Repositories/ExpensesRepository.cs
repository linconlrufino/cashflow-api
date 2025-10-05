using Domain.Entities;
using Domain.Repositories.Expenses;
using Infrastructure.DataAccess;

namespace Infrastructure.Repositories;

internal class ExpensesRepository : IExpensesRepository
{
    public void Add(Expense expense)
    {
        var dbContext = new CashFlowDbContext();

        dbContext.Expenses.Add(expense);
        dbContext.SaveChanges();
    }
}