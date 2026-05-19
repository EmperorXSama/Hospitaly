using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Users.Domain.Users;

namespace Hospitaly.Modules.Users.Application.Users.Commands.AssignRole;

public record AssignRoleCommand (string IdentityId , Role Role): ICommand;