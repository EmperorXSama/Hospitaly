using ErrorOr;

namespace Hospitaly.Modules.Users.Application.Users.Commands.RegisterUser;

public static class IdentityProviderErrors
{
    public static readonly Error EmailIsNotUnique = Error.Conflict(
        "Identity.EmailIsNotUnique",
        "The specified email is not unique.");
}