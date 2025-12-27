using Domain.Enums;

namespace Domain.Entities;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Guid  UserIdentifier { get; set; }
    public string Role { get; set; } = Roles.TEAM_MEMBER;
}