using Domain.Entities;

namespace Domain.Security.Token;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}