using Communication.Requests.User;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Users;
using Domain.Security.Cryptography;
using Domain.Services.LoggedUser;
using Exception;
using Exception.ExceptionsBase;
using FluentValidation.Results;

namespace Application.UseCases.Users.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser loggedUser;
    private readonly IUserUpdateOnlyRepository userUpdateOnlyRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IPasswordEncripter passwordEncripter;

    public ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IUnitOfWork unitOfWork,
        IPasswordEncripter passwordEncripter)
    {
        this.loggedUser = loggedUser;
        this.userUpdateOnlyRepository = userUpdateOnlyRepository;
        this.unitOfWork = unitOfWork;
        this.passwordEncripter = passwordEncripter;
    }

    public async Task Execute(ChangePasswordRequest request)
    {
        var loggedUserInfo = await loggedUser.Get();
        
        await Validate(request, loggedUserInfo);
        
        var user = await userUpdateOnlyRepository.GetUserById(loggedUserInfo.Id);
        user.Password = passwordEncripter.Encrypt(request.NewPassword);
        
        userUpdateOnlyRepository.Update(user);
        
        await unitOfWork.Commit();
    }

    private async Task Validate(ChangePasswordRequest request, User loggedUserInfo)
    {
        var validator = new ChangePasswordValidator();
        var result = await validator.ValidateAsync(request);

        var passwordMatch = passwordEncripter.Verify(request.Password, loggedUserInfo.Password);
        
        if(passwordMatch is false)
            result.Errors.Add(new ValidationFailure(string.Empty,ResourcesErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        if (result.IsValid is false)
        {
            var erros = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(erros);
        }
    }
}