using Communication.Requests;
using Communication.Requests.User;

namespace Application.UseCases.Users.Update;

public interface IUpdateUserUseCase
{
    Task Execute(UpdateUserRequest request);
}