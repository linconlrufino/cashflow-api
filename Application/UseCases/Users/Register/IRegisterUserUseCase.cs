using Communication.Requests.Expense;
using Communication.Responses.User;

namespace Application.UseCases.Users.Register;

public interface IRegisterUserUseCase
{
    Task<RegisteredUserResponse> Execute(RegisterUserRequest request);
}