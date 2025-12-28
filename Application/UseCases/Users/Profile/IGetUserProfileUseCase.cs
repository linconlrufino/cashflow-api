using Communication.Responses.User;

namespace Application.UseCases.Users.Profile;

public interface IGetUserProfileUseCase
{
    Task<UserProfileResponse> Execute();
}