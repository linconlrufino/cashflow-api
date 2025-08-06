using Communication.Requests;

namespace Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase
{
    public RegisterExpenseRequest Execute(RegisterExpenseRequest request)
    {
        Validate(request);
        return new RegisterExpenseRequest();
    }

    private void Validate(RegisterExpenseRequest request)
    { 
        var validator = new RegisterExpenseValidator();
        var result = validator.Validate(request);
    } 
}