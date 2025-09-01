using Application.UseCases.Expenses.Register;
using Communication.Enums;
using Communication.Requests;

namespace Validators.Tests.Expenses.Register;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new RegisterExpenseValidator();
        var request = new RegisterExpenseRequest()
        {
            Amount = 100,
            Date = DateTime.Now.AddDays(-1),
            Description = "description",
            Title = "Apple",
            PaymentType = PaymentType.CreditCard
        };
        
        //Act
        var result = validator.Validate(request);
        
        //Assert
        Assert.True(result.IsValid);
    }
}