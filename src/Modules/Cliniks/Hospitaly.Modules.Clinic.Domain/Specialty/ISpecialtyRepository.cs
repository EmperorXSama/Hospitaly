namespace Hospitaly.Modules.Clinic.Domain.Specialty;

public interface ISpecialtyRepository
{
  Task<IEnumerable<Specialty>> GetAllAsync(CancellationToken cancellationToken);
}