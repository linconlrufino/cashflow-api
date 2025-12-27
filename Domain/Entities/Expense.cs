using Domain.Enums;

namespace Domain.Entities;

public class Expense
{
    public long Id { get; private set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }

    public long UserId  { get; set; }
    public User User { get; set; }
}