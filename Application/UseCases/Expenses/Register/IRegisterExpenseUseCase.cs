using Communication.Requests;

namespace Application.UseCases.Expenses.Register;

public interface IRegisterExpenseUseCase
{
    Task<RegisterExpenseRequest> Execute(RegisterExpenseRequest request);
}