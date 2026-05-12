namespace Hospitaly.Modules.Users.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetUserAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(User user);
}