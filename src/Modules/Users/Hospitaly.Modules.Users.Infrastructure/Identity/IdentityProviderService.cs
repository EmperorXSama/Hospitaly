using ErrorOr;
using Hospitaly.Modules.Users.Application.Abstractions.Data;
using Hospitaly.Modules.Users.Application.Abstractions.Identity;
using Hospitaly.Modules.Users.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Hospitaly.Modules.Users.Infrastructure.Identity;

public class IdentityProviderService(
    KeyCloakClient htpClient,
    ILogger<IdentityProviderService> logger
    )  : IIdentityProviderService
{
    private const string PasswordCredentialType = "password";
    public async Task<ErrorOr<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
       var userRepresentation =  new UserRepresentation(user.Email, user.Email, user.FirstName,
            user.LastName, true, true, new[]
            {

                new CredentialRepresentation(PasswordCredentialType, user.Password, false)
            });

       try
       {
           var identityId = await htpClient.RegisterUserAsync(userRepresentation
               , cancellationToken);

           return identityId;
       }
       catch (Exception e)
       {
           logger.LogError($"Something went wrong registering a user \n {e.Message}");
           throw;
       }
  }
}