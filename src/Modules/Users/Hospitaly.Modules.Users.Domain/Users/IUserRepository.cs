namespace Hospitaly.Modules.Users.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdentity(string identityId, CancellationToken cancellationToken = default);
    void AttachRole(Role role);
    void Insert(User user);
}