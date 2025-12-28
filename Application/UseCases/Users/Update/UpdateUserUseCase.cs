using AutoMapper;
using Communication.Requests.User;
using Domain.Repositories;
using Domain.Repositories.Users;
using Domain.Services.LoggedUser;
using Exception;
using Exception.ExceptionsBase;
using FluentValidation.Results;

namespace Application.UseCases.Users.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser loggedUser;
    private readonly IUserUpdateOnlyRepository userUpdateOnlyRepository;
    private readonly IUsersReadOnlyRepository usersReadOnlyRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    
    public UpdateUserUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IUsersReadOnlyRepository usersReadOnlyRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        this.loggedUser = loggedUser;
        this.userUpdateOnlyRepository = userUpdateOnlyRepository;
        this.usersReadOnlyRepository = usersReadOnlyRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task Execute(UpdateUserRequest request)
    { 
        var loggedUserInfo = await loggedUser.Get();
        
        await Validate(request, loggedUserInfo.Email);

        var user = await userUpdateOnlyRepository.GetUserById(loggedUserInfo.Id);
       
        mapper.Map(request, user);
       
        userUpdateOnlyRepository.Update(user);
       
        await unitOfWork.Commit();
    }

    private async Task Validate(UpdateUserRequest request, string currentEmail)
    {
        var validator = new UpdateUserValidator();
        var result = await validator.ValidateAsync(request);

        if (currentEmail.Equals(request.Email) is false)
        {
            var userExists = await usersReadOnlyRepository.ExistsActiveUserWithEmail(request.Email);
            if(userExists)
                result.Errors.Add(new ValidationFailure(string.Empty, ResourcesErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}