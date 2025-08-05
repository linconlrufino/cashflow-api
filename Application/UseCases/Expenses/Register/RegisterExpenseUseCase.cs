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
        if(string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Title is required.");
        if(request.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");
        if (request.Date.CompareTo(DateTime.Today) > 0)
            throw new ArgumentException("Expenses cannot be for the future.");
        if(!Enum.IsDefined(request.PaymentType))
            throw new ArgumentException("Payment type is not valid.");
    } 
}