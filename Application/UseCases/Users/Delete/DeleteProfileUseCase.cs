using Domain.Repositories;
using Domain.Repositories.Users;
using Domain.Services.LoggedUser;

namespace Application.UseCases.Users.Delete;

public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly ILoggedUser loggedUser;
    private readonly IUsersWriteOnlyRepository usersWriteOnlyRepository;
    private readonly IUnitOfWork unitOfWork;

    public DeleteUserAccountUseCase(
        ILoggedUser loggedUser,
        IUsersWriteOnlyRepository usersWriteOnlyRepository,
        IUnitOfWork unitOfWork)
    { 
        this.loggedUser = loggedUser;
        this.usersWriteOnlyRepository = usersWriteOnlyRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task Execute()
    {
        var user = await loggedUser.Get();
        await usersWriteOnlyRepository.Delete(user);
        await unitOfWork.Commit();
    }
}