using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

internal class CashFlowDbContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = "Server=localhost;Database=cashflow;Uid=root;Pwd=271198;";
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 43));
        optionsBuilder.UseMySql(connectionString, serverVersion);
    }
}