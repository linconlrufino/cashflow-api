namespace Communication.Responses;

public class ErrorResponse
{
    public ErrorResponse(List<string> errorMessage)
    {
        ErrorMessages = errorMessage;
    }

    public ErrorResponse(string errorMessage)
    {
        ErrorMessages = [errorMessage];
    }
    
    public List<string> ErrorMessages { get; set; }
}