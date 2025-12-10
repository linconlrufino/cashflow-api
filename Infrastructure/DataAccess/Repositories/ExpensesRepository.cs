using Domain.Entities;
using Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
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

    async Task<Expense?> IExpensesReadOnlyRepository.GetByIdAsync(long id)
    {
        return await dbContext.Expenses
            .AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id == id);
    }

    public async Task<IEnumerable<Expense>> FilterByMonth(DateOnly date)
    {
        return await dbContext.Expenses
            .AsNoTracking()
            .Where(expense => expense.Date.Month == date.Month &&
                              expense.Date.Year == date.Year)
            .OrderBy(expense => expense.Date)   
            .ToArrayAsync();
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetByIdAsync(long id)
    {
        return await dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
    }
    
    public void Update(Expense expense)
    {
        dbContext.Expenses.Update(expense);
    }
}