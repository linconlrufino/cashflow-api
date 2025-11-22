using Application.UseCases.Expenses;
using CommonTestUtilities.Requests;
using Communication.Enums;
using Exception;

namespace Validators.Tests.Expenses.Register;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RegisterExpenseRequestBuilder.Build();
        
        //Act
        var result = validator.Validate(request);
        
        //Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Error_Title_Empty(string title)
    {
        var validator = new ExpenseValidator();
        var request = RegisterExpenseRequestBuilder.Build();
        request.Title = title;

        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, error => error.ErrorMessage.Equals(ResourcesErrorMessages.TITLE_REQUIRED));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Error_Amount_GreaterThan_Zero(decimal amount)
    {
        var validator = new ExpenseValidator();
        var request = RegisterExpenseRequestBuilder.Build();
        request.Amount = amount;

        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, error => error.ErrorMessage.Equals(ResourcesErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    }
    
    [Fact]
    public void Error_Date_Cannot_Be_In_The_Future()
    {
        var validator = new ExpenseValidator();
        var request = RegisterExpenseRequestBuilder.Build();
        request.Date = DateTime.Today.AddDays(1);

        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, error => error.ErrorMessage.Equals(ResourcesErrorMessages.EXPENSES_CANNOT_BE_FOR_THE_FUTURE));
    }
    
    [Fact]
    public void Error_PaymentType_Invalid()
    {
        var validator = new ExpenseValidator();
        var request = RegisterExpenseRequestBuilder.Build();
        request.PaymentType = (PaymentType)99;

        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, error => error.ErrorMessage.Equals(ResourcesErrorMessages.PAYMENT_TYPE_IS_NOT_VALID));
    }
}