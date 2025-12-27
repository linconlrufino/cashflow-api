using System.Net;

namespace Exception.ExceptionsBase;

public class InvalidLoginException: CashFlowException
{
    public InvalidLoginException() : base(ResourcesErrorMessages.EMAIL_OR_PASSWORD_INVALID) { }
    
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;
    public override List<string> GetErrors()
    {
        return [Message];
    }
}