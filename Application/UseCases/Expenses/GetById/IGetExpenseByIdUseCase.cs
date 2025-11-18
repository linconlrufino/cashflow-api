using Communication.Responses;

namespace Application.UseCases.Expenses.GetById;

public interface IGetExpenseByIdUseCase
{
    Task<ShortExpenseResponse> Execute(int expenseId);
}