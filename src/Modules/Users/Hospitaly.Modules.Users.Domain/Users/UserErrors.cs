using ErrorOr;
namespace Hospitaly.Modules.Users.Domain.Users;

public static class UserErrors
{
    public static Error UserNotFound(Guid userId) => 
        Error.NotFound("Users.NotFound", $"The user with the identifier {userId} not found");
    public static Error UserNotFound(string identityId) => 
        Error.NotFound("Users.NotFound", $"The user with the IDP identifier {identityId} not found");
}