using Hospitaly.Common.Domain;

namespace Hospitaly.Modules.Users.Domain.Users.Events;

public sealed class UserRegisteredDomainEvent(Guid userId):DomainEvent
{
    public Guid UserId { get; init; } = userId;
    
}