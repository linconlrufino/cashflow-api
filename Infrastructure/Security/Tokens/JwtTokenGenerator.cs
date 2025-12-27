using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security.Tokens;

public class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly uint expirationTimeMinutes;
    private readonly string signingKey;

    public JwtTokenGenerator(uint expirationTimeMinutes, string signingKey)
    {
        this.expirationTimeMinutes = expirationTimeMinutes;
        this.signingKey = signingKey;
    }

    public string Generate(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(GetClaims(user))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken= tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }

    private static List<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Sid, user.UserIdentifier.ToString()),
        };
        return claims;
    }

    private SymmetricSecurityKey GetSecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(signingKey);
        return new SymmetricSecurityKey(key);
    }
}