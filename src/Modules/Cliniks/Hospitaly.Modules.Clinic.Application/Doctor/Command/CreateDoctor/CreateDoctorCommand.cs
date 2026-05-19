using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.CreateDoctor;

public sealed record CreateDoctorCommand(Guid UserId) : ICommand<Guid>;
