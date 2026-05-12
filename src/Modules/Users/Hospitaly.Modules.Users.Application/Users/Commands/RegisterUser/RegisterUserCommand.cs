using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Users.Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Sex,
    DateOnly DateOfBirth,
    string? BloodType) : ICommand<Guid>;