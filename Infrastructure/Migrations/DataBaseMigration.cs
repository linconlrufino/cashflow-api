using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Migrations;

public static class DataBaseMigration
{
    public static async Task MigrateDatabaseAsync(IServiceProvider serviceProvider)
    {
       var dbContext = serviceProvider.GetRequiredService<CashFlowDbContext>();

       await dbContext.Database.MigrateAsync();
    }
} 