using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Entities;
using Domain.Security.Tokens;
using Domain.Services.LoggedUser;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.LoggedUser;

internal class LoggedUser : ILoggedUser
{
    private readonly CashFlowDbContext dbContext;
    private readonly ITokenProvider tokenProvider;

    public LoggedUser(CashFlowDbContext dbContext, ITokenProvider tokenProvider)
    {
        this.dbContext = dbContext;
        this.tokenProvider = tokenProvider;
    }
    
    public async Task<User> Get()
    {
        var token = tokenProvider.TokenOnRequest();
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;
        
        return await dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.UserIdentifier == Guid.Parse(identifier));
    }
}