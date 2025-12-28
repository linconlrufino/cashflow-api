using Communication.Requests.Expense;
using Communication.Responses.User;
using Domain.Repositories.Users;
using Domain.Security.Cryptography;
using Domain.Security.Tokens;
using Exception.ExceptionsBase;

namespace Application.UseCases.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUsersReadOnlyRepository usersReadOnlyRepository;
    private readonly IPasswordEncripter passwordEncripter; 
    private readonly IAccessTokenGenerator accessTokenGenerator;

    public DoLoginUseCase(
        IUsersReadOnlyRepository usersReadOnlyRepository,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        this.usersReadOnlyRepository = usersReadOnlyRepository;
        this.passwordEncripter = passwordEncripter;
        this.accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<RegisteredUserResponse> Execute(LoginRequest request)
    {
        var user = await usersReadOnlyRepository.GetUserByEmail(request.Email);

        if (user is null)
            throw new InvalidLoginException();

        if(!passwordEncripter.Verify(request.Password, user.Password))
            throw new InvalidLoginException();

        return new RegisteredUserResponse
        {
            Name = user.Name,
            Token = accessTokenGenerator.Generate(user)
        };
    }
}