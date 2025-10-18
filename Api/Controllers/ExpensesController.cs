using Application.UseCases.Expenses.Register;
using Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost()]
    public IActionResult Register(
        [FromServices] IRegisterExpenseUseCase useCase,
        [FromBody] RegisterExpenseRequest request)
    {
        var response = useCase.Execute(request);
        return Created(string.Empty, response);
    }
}