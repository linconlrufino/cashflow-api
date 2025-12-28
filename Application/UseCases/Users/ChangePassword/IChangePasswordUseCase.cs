using Communication.Requests.User;

namespace Application.UseCases.Users.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(ChangePasswordRequest request);
}