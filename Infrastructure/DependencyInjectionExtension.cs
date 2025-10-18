using Domain.Repositories.Expenses;
using Infrastructure.DataAccess;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        AddDbContext(services);
        AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IExpensesRepository, ExpensesRepository>();
    }
    
    private static void AddDbContext(IServiceCollection services)
    {
        services.AddDbContext<CashFlowDbContext>();
    }
}