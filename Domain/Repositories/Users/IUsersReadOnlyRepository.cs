using Domain.Entities;

namespace Domain.Repositories.Users;

public interface IUsersReadOnlyRepository
{
    Task<bool> ExistsActiveUserWithEmail(string email);
    Task<User?> GetUserByEmail(string email);
}