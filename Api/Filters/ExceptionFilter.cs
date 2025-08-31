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
        if (context.Exception is not ErrorOnValidationException errorOnValidationException)
            return;

        var errorResponse = new ErrorResponse(errorOnValidationException.Errors);

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