using Communication.Requests;

namespace Application.UseCases.Expenses.Register;

public interface IRegisterExpenseUseCase
{
    RegisterExpenseRequest Execute(RegisterExpenseRequest request);
}