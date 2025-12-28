using Domain.Entities;
using Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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

    public async Task<bool> DeleteAsync(long userId, long id)
    {
       var result =  await dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == userId);
       
       if (result is null)
           return false;
       
       dbContext.Expenses.Remove(result);
       return true;
    }

    public async Task<IEnumerable<Expense>> GetAllAsync(long userId)
    {
        return await dbContext.Expenses.AsNoTracking().Where(expense => expense.UserId == userId).ToArrayAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetByIdAsync(long userId, long id)
    {
        return await GetFullExpense()
            .AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == userId);
    }
    async Task<Expense?> IExpensesUpdateOnlyRepository.GetByIdAsync(long userId, long id)
    {
        return await GetFullExpense().FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == userId);
    }
    
    public void Update(Expense expense)
    {
        dbContext.Expenses.Update(expense);
    }
    
    public async Task<IEnumerable<Expense>> FilterByMonth(long userId, DateOnly date)
    {
        return await dbContext.Expenses
            .AsNoTracking()
            .Where(expense => 
                expense.UserId == userId && 
                expense.Date.Month == date.Month && 
                expense.Date.Year == date.Year)
            .OrderBy(expense => expense.Date)   
            .ToArrayAsync();
    }

    public IIncludableQueryable<Expense, ICollection<Tag>> GetFullExpense()
    {
        return dbContext.Expenses
            .Include(expense => expense.Tags);
    }
}