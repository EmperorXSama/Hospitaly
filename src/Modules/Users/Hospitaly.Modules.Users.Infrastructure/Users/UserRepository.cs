using Hospitaly.Modules.Users.Domain.Users;
using Hospitaly.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hospitaly.Modules.Users.Infrastructure.Users;

public class UserRepository(UserDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetUserByIdentity(string identityId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(u => u.Roles)  
            .SingleOrDefaultAsync(u => u.IdentityId == identityId, cancellationToken);
    }
    public void AttachRole(Role role)
    {
        if (dbContext.Entry(role).State == EntityState.Detached)
            dbContext.Attach(role);
    }
    public void Insert(User user)
    {
        foreach (var userRole in user.Roles)
        {
            dbContext.Attach(userRole);
        }

        dbContext.Users.Add(user);
    }
}