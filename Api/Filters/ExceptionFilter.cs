using Communication.Responses;
using Exception;
using Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlowApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CashFlowException)
            HandleProjectException(context);
        else
            ThrowUnknownError(context);
    }

    private static void HandleProjectException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ErrorOnValidationException errorOnValidationException:
                ThrowErrorOnValidationException(context, errorOnValidationException);
                break;
            case NotFoundException notFoundException:
                ThrowNotFoundException(context, notFoundException);
                break;
            default:
                ThrowBadRequestException(context);
                break;
        }
    }

    private static void ThrowErrorOnValidationException(ExceptionContext context,ErrorOnValidationException errorOnValidationException)
    {
        var errorResponse = new ErrorResponse(errorOnValidationException.Errors);

        context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Result = new BadRequestObjectResult(errorResponse);
    }
    
    private static void ThrowNotFoundException(ExceptionContext context, NotFoundException notFoundException)
    {
        var errorResponse = new ErrorResponse(notFoundException.Message);

        context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        context.Result = new NotFoundObjectResult(errorResponse);
    }
    
    private static void ThrowBadRequestException(ExceptionContext context)
    {
        var errorResponse = new ErrorResponse(context.Exception.Message);

        context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Result = new BadRequestObjectResult(errorResponse);
    }
    
    private static void ThrowUnknownError(ExceptionContext context)
    {
        var errorResponse = new ErrorResponse(ResourcesErrorMessages.UNKNOWN_ERROR);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}