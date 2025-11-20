using Communication.Responses;

namespace Application.UseCases.Expenses.GetById;

public interface IGetExpenseByIdUseCase
{
    Task<ExpenseResponse> Execute(long id);
}