using Domain.Enums;

namespace Domain.Entities;

public class Tag
{
    public long Id { get; set; }
    public TagType TagType { get; set; }
    public long ExpenseId { get; set; }
    
    public Expense Expense { get; set; }
}