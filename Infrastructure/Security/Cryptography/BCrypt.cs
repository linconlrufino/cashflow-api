using Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace Infrastructure.Security;

public class BCrypt : IPasswordEncripter
{
    public string Encrypt(string password) 
        => BC.HashPassword(password);

    public bool Verify(string password, string passwordHash)
        => BC.Verify(password, passwordHash);
}