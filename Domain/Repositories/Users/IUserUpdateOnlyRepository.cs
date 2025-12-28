using Domain.Entities;

namespace Domain.Repositories.Users;

public interface IUserUpdateOnlyRepository
{
    Task<User> GetUserById(long id);
    void Update(User user);
}