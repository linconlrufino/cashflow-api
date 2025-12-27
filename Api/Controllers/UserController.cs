using Application.UseCases.Users.Register;
using Communication.Requests;
using Communication.Responses;
using Communication.Responses.User;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    
    [HttpPost]
    [ProducesResponseType(typeof(RegisteredUserResponse),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RegisterUserRequest request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
}  