using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Doctor;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.CreateDoctor;

internal sealed class CreateDoctorCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateDoctorCommand, Guid>
{
    public async Task<ErrorOr<Guid>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = Doctor.Create(request.UserId, DateTime.UtcNow);
        if (doctor.IsError)
            return doctor.Errors;

        doctorRepository.Insert(doctor.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return doctor.Value.Id;
    }
}
