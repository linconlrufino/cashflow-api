using Communication.Requests;
using Communication.Responses.User;

namespace Application.UseCases.Login;

public interface IDoLoginUseCase
{
    Task<RegisteredUserResponse> Execute(LoginRequest request);
}