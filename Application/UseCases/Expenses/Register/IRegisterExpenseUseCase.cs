using Communication.Requests;
using Communication.Responses;

namespace Application.UseCases.Expenses.Register;

public interface IRegisterExpenseUseCase
{
    Task<RegisteredExpenseResponse> Execute(RegisterExpenseRequest request);
}