using Communication.Enums;

namespace Communication.Requests;

public class RegisterExpenseRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public Decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }
}