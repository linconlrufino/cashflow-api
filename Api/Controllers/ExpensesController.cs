using Application.UseCases.Expenses.Register;
using Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost()]
    public async Task<ActionResult> Register(
        [FromServices] IRegisterExpenseUseCase useCase,
        [FromBody] RegisterExpenseRequest request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
}