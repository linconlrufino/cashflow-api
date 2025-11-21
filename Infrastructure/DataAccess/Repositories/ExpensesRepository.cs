using Domain.Entities;
using Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository
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

    public async Task<bool> DeleteAsync(long id)
    {
       var result =  await dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
       if (result is null)
           return false;
       
       dbContext.Expenses.Remove(result);

       return true;
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