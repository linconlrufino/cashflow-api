using Application;
using CashFlowApi.Filters;
using CashFlowApi.Middleware;
using Infrastructure;
using Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await MigrationDatabase();

app.Run();

async Task MigrationDatabase()
{
    await using var scope = app.Services.CreateAsyncScope();

    await DataBaseMigration.MigrateDatabaseAsync(scope.ServiceProvider);
}