using Domain.Entities;
using Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal class UsersRepository : IUsersReadOnlyRepository, IUsersWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly CashFlowDbContext dbContext;

    public UsersRepository(CashFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task<bool> ExistsActiveUserWithEmail(string email)
    {
        return await dbContext.Users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task Add(User user)
    { 
        await dbContext.Users.AddAsync(user);
    }

    public async Task<User> GetUserById(long id)
    {
        return await dbContext.Users.FirstAsync(user => user.Id == id);
    }

    public void Update(User user)
    {
        dbContext.Users.Update(user);
    }
}