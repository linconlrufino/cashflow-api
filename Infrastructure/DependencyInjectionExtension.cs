using Domain.Repositories.Expenses;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IExpensesRepository, ExpensesRepository>();
    }
}