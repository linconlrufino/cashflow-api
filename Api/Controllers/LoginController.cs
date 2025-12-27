using Application.UseCases.Login;
using Communication.Requests;
using Communication.Responses;
using Communication.Responses.User;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost()]
    [ProducesResponseType(typeof(RegisteredUserResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase,
        [FromBody] LoginRequest request)
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }
}