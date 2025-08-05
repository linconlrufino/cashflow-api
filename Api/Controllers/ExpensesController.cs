using Application.UseCases.Expenses.Register;
using Communication.Requests;
using Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost()]
    public IActionResult Register(RegisterExpenseRequest request)
    {
        try
        {
            var useCase = new RegisterExpenseUseCase();
            var response = useCase.Execute(request);
            return Created(string.Empty, response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new ErrorResponse(ex.Message);
            return BadRequest(errorResponse);
        }
        catch
        {
            var errorResponse = new ErrorResponse("Unknown Error");
            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }
}