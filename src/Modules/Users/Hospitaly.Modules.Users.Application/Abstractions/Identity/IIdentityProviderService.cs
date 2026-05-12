using ErrorOr;

namespace Hospitaly.Modules.Users.Application.Abstractions.Identity;

public interface IIdentityProviderService
{
    Task<ErrorOr<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
}