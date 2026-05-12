namespace Hospitaly.Modules.Clinic.Domain.Clinic;

public interface IClinicRepository
{
    Task<Clinic?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Insert(Clinic clinic);
}