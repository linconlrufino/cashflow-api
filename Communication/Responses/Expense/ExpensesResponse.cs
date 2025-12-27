namespace Communication.Responses;

public class ExpensesResponse
{
    public IEnumerable<ShortExpenseResponse> Expenses { get; set; } = [];
}