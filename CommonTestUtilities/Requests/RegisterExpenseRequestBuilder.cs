using Bogus;
using Communication.Enums;
using Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RegisterExpenseRequestBuilder
{
    public static ExpenseRequest Build()
    {
        return new Faker<ExpenseRequest>()
            .RuleFor(request => request.Title, faker => faker.Commerce.ProductName())
            .RuleFor(request => request.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(request => request.Date, faker => faker.Date.Past())
            .RuleFor(request => request.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(request => request.Amount, faker => faker.Finance.Amount(min: 1));
    }
}