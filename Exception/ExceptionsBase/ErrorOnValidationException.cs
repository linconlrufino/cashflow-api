namespace Exception.ExceptionsBase;

public class ErrorOnValidationException : CashFlowException
{
    public ErrorOnValidationException(List<string> errorMessages)
    {
        Errors = errorMessages;
    }

    public List<string> Errors { get; set; }
}