using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Users.Application.Users.Commands.CompleteOnboarding;

public sealed record CompleteOnboardingCommand(Guid UserId) : ICommand<Success>;
