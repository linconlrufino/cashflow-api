using AutoMapper;
using Communication.Responses.User;
using Domain.Services.LoggedUser;

namespace Application.UseCases.Users.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly IMapper mapper;
    private readonly ILoggedUser loggedUser;

    public GetUserProfileUseCase(
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        this.mapper = mapper;
        this.loggedUser = loggedUser;
    }

    public async Task<UserProfileResponse> Execute()
    {
        var loggedUserInfo = await loggedUser.Get();

        return mapper.Map<UserProfileResponse>(loggedUserInfo);
    }
}