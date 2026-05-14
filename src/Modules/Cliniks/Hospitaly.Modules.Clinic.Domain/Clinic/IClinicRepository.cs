namespace Hospitaly.Modules.Clinic.Domain.Clinic;

public interface IClinicRepository
{
    Task<Clinic?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Clinic?> GetByIdWithInclude(Guid id, CancellationToken cancellationToken = default);
    void Insert(Clinic clinic);
}