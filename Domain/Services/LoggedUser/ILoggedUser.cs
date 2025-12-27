using Domain.Entities;

namespace Domain.Services.LoggedUser;

public interface ILoggedUser
{
    Task<User> Get();
}