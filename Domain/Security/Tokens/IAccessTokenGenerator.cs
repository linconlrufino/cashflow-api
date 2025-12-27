using Domain.Entities;

namespace Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}