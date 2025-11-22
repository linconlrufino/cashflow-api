using Communication.Requests;

namespace Application.UseCases.Expenses.Update;

public interface IUpdateExpenseUseCase
{
    Task Execute(long id, ExpenseRequest request);
}