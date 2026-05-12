using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Users.Domain.Users.Events;

namespace Hospitaly.Modules.Users.Domain.Users;

public class User : AggregateRoot
{
    private User() { }

    public string IdentityId { get; private set; }
    public string Email { get; private set; }
    public string FirsName { get; private set; }
    public string LastName { get; private set; }
    public bool RequiresOnboarding { get; private set; } = true;
    public Sex Sex { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public BloodType? BloodType { get; private set; }

    private readonly List<Role> _roles = [];
    public IReadOnlyCollection<Role> Roles => _roles.ToList();

    public void CompleteOnboarding()
    {
        RequiresOnboarding = false;
    }

    public static ErrorOr<User> Create(
        string identityId,
        string email,
        string firstName,
        string lastName,
        Sex sex,
        DateOnly dateOfBirth,
        BloodType? bloodType = null
        )
    {
        var user = new User()
        {
            FirsName = firstName,
            LastName = lastName,
            Email = email,
            IdentityId = identityId,
            Sex = sex,
            DateOfBirth = dateOfBirth,
            BloodType = bloodType
        };
        user._roles.Add(Role.Member);
        user.RequiresOnboarding = true;
        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id));

        return user;
    }
}