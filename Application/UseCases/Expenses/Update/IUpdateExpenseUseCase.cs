using Communication.Requests;
using Communication.Requests.Expense;

namespace Application.UseCases.Expenses.Update;

public interface IUpdateExpenseUseCase
{
    Task Execute(long id, ExpenseRequest request);
}