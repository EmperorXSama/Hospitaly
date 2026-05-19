using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Doctor;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.AddDoctorSpecialty;

internal sealed class AddDoctorSpecialtyCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddDoctorSpecialtyCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        AddDoctorSpecialtyCommand request,
        CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByIdWithIncludeAsync(request.DoctorId, cancellationToken);
        if (doctor is null)
            return DoctorErrors.DoctorNotFound(request.DoctorId);

        var specialties = new List<DoctorSpecialty>();
        foreach (var item in request.Specialties)
        {
            var result = DoctorSpecialty.Create(
                request.DoctorId,
                item.SpecialtyId,
                item.IsPrimary,
                item.CertificationNumber,
                item.CertifiedAt);

            if (result.IsError)
                return result.Errors;

            specialties.Add(result.Value);
        }

        var addResult = doctor.AddSpecialties(specialties);
        if (addResult.IsError)
            return addResult.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
