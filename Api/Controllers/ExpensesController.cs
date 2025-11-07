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
    [ProducesResponseType(typeof(RegisteredExpenseResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register(
        [FromServices] IRegisterExpenseUseCase useCase,
        [FromBody] RegisterExpenseRequest request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
}