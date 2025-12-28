using Domain.Entities;

namespace Domain.Repositories.Users;

public interface IUsersWriteOnlyRepository
{
    Task Add(User user);
    Task Delete(User user);
}