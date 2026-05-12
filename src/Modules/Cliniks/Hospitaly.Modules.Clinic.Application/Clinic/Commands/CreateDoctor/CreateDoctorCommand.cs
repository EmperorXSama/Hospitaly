using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.CreateDoctor;

public sealed record CreateDoctorCommand(Guid UserId) : ICommand<Guid>;
