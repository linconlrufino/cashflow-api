using Communication.Responses;

namespace Application.UseCases.Expenses.GetAll;

public interface IGetAllExpensesUseCase
{
    Task<ExpensesResponse> Execute();
}