using Domain.Repositories;

namespace Infrastructure.DataAccess;

internal class UnitOfWork : IUnitOfWork
{
    private readonly CashFlowDbContext context;

    public UnitOfWork(CashFlowDbContext context)
    {
        this.context = context;
    }

    public async Task Commit()
        => await context.SaveChangesAsync();
}