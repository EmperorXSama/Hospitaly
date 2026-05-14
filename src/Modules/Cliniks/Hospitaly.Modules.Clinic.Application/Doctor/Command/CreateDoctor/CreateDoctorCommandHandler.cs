using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Doctor;
using PublicApi;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.CreateDoctor;
// Global mark . note : the request.UserId.ToString() is stupid , it needs to be passed as strign from the controller without convert it to guid just to make it string again :0 .
internal sealed class CreateDoctorCommandHandler(
    IDoctorRepository doctorRepository,
    IUserApi userApi,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateDoctorCommand, Guid>
{
    public async Task<ErrorOr<Guid>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = Domain.Doctor.Doctor.Create(request.UserId, DateTime.UtcNow);
        if (doctor.IsError)
            return doctor.Errors;

        doctorRepository.Insert(doctor.Value);
        await userApi.AddDoctorRole(request.UserId.ToString() , cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return doctor.Value.Id;
    }
    
    /*
        * note: same note of assigning roles i wrote in Create clinic command applies for this command as well :) am lazy 
     */
}
