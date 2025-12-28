using AutoMapper;
using Communication.Requests.Expense;
using Communication.Responses.User;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Users;
using Domain.Security.Cryptography;
using Domain.Security.Tokens;
using Exception;
using Exception.ExceptionsBase;

namespace Application.UseCases.Users.Register;

public class RegisterUserUseCase: IRegisterUserUseCase
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;
    private readonly IPasswordEncripter passwordEncripter;
    private readonly IUsersReadOnlyRepository usersReadOnlyRepository;
    private readonly IUsersWriteOnlyRepository usersWriteOnlyRepository;
    private readonly IAccessTokenGenerator accessTokenGenerator;

    public RegisterUserUseCase(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IPasswordEncripter passwordEncripter,
        IUsersReadOnlyRepository usersReadOnlyRepository,
        IUsersWriteOnlyRepository usersWriteOnlyRepository,
        IAccessTokenGenerator accessTokenGenerator)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
        this.passwordEncripter = passwordEncripter;
        this.usersReadOnlyRepository = usersReadOnlyRepository;
        this.usersWriteOnlyRepository = usersWriteOnlyRepository;
        this.accessTokenGenerator = accessTokenGenerator;
    }
    
    public async Task<RegisteredUserResponse> Execute(RegisterUserRequest request)
    {
        await Validate(request);

        var user = mapper.Map<User>(request);
        user.Password = passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();
        
        await usersWriteOnlyRepository.Add(user);
        await unitOfWork.Commit();
        
        return new RegisteredUserResponse
        {
            Name = user.Name,
            Token = accessTokenGenerator.Generate(user)
        };
    }

    private async Task Validate(RegisterUserRequest request)
    {
        var result = new UserValidator().Validate(request);
        
        if(await usersReadOnlyRepository.ExistsActiveUserWithEmail(request.Email))
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourcesErrorMessages.EMAIL_ALREADY_REGISTERED));
        
        if (result.IsValid)
            return;
        
        var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}