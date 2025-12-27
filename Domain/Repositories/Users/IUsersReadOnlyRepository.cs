namespace Domain.Repositories.Users;

public interface IUsersReadOnlyRepository
{
    Task<bool> ExistsActiveUserWithEmail(string email);
}