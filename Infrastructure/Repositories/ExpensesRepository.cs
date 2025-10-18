using Domain.Entities;
using Domain.Repositories.Expenses;
using Infrastructure.DataAccess;

namespace Infrastructure.Repositories;

internal class ExpensesRepository : IExpensesRepository
{
    private readonly CashFlowDbContext dbContext;
    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Add(Expense expense)
    {
        dbContext.Expenses.Add(expense);
        dbContext.SaveChanges();
    }
}