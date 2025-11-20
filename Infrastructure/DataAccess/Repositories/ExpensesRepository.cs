using Domain.Entities;
using Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository : IExpensesRepository
{
    private readonly CashFlowDbContext dbContext;

    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(Expense expense)
    {
        await dbContext.Expenses.AddAsync(expense);
    }

    public async Task<IEnumerable<Expense>> GetAllAsync()
    {
        return await dbContext.Expenses.AsNoTracking().ToArrayAsync();
    }

    public async Task<Expense?> GetByIdAsync(long id)
    {
        return await dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id);
    }
}