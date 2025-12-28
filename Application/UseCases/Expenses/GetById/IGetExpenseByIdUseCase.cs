using Communication.Responses;
using Communication.Responses.Expense;

namespace Application.UseCases.Expenses.GetById;

public interface IGetExpenseByIdUseCase
{
    Task<ExpenseResponse> Execute(long id);
}