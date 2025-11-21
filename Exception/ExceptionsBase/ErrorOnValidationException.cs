using System.Net;

namespace Exception.ExceptionsBase;

public class ErrorOnValidationException : CashFlowException
{
    private readonly List<string> Errors;

    public ErrorOnValidationException(List<string> errorMessages) : base(string.Empty)
    {
        Errors = errorMessages;
    }

    public override int StatusCode => (int)HttpStatusCode.BadRequest;
    public override List<string> GetErrors() => Errors;
}