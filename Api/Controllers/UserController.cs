using Application.UseCases.Users.Profile;
using Application.UseCases.Users.Register;
using Application.UseCases.Users.Update;
using Communication.Requests;
using Communication.Requests.Expense;
using Communication.Requests.User;
using Communication.Responses;
using Communication.Responses.User;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(UserProfileResponse),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var response = await useCase.Execute();
        return Ok(response);
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProfile(
        [FromServices] IUpdateUserUseCase useCase,
        [FromBody] UpdateUserRequest request)
    {
        await useCase.Execute(request);
        return NoContent();
    }
}  