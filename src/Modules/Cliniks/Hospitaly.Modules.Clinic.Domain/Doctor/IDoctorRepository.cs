namespace Hospitaly.Modules.Clinic.Domain.Doctor;

public interface IDoctorRepository
{
    Task<Doctor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Insert(Doctor doctor);
}
