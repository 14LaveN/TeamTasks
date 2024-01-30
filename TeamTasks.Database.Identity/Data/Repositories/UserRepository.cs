using Microsoft.EntityFrameworkCore;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.ValueObjects;
using IUserRepository = TeamTasks.Database.Identity.Data.Interfaces.IUserRepository;

namespace TeamTasks.Database.Identity.Data.Repositories;

public class UserRepository(UserDbContext userDbContext)
    : IUserRepository
{
    public async Task<Maybe<User>> GetByIdAsync(Guid userId) =>
            await userDbContext.Set<User>().FirstOrDefaultAsync(x=>x.Id == userId) 
            ?? throw new ArgumentNullException();

    public async Task<Maybe<User>> GetByNameAsync(string name) =>
        await userDbContext.Set<User>().FirstOrDefaultAsync(x=>x.UserName == name) 
        ?? throw new ArgumentNullException();

    public async Task<Maybe<User>> GetByEmailAsync(EmailAddress emailAddress) =>
        await userDbContext.Set<User>().FirstOrDefaultAsync(x=>x.EmailAddress == emailAddress) 
        ?? throw new ArgumentNullException();

    public Task<bool> IsEmailUniqueAsync(EmailAddress emailAddress)
    {
        throw new NotImplementedException();
    }
}