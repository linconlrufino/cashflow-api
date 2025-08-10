using Application.UseCases.Expenses.Register;
using Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost()]
    public IActionResult Register(RegisterExpenseRequest request)
    {
        var useCase = new RegisterExpenseUseCase();
        var response = useCase.Execute(request);
        return Created(string.Empty, response);
    }
}