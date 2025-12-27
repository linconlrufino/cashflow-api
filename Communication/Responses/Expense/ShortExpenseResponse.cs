using Communication.Enums;

namespace Communication.Responses;

public class ShortExpenseResponse
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Decimal Amount { get; set; }
}