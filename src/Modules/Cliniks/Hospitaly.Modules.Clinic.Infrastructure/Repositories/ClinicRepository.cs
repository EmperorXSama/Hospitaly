using Hospitaly.Modules.Clinic.Domain.Clinic;
using Hospitaly.Modules.Clinic.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using ClinicEntity = Hospitaly.Modules.Clinic.Domain.Clinic.Clinic;

namespace Hospitaly.Modules.Clinic.Infrastructure.Repositories;

public class ClinicRepository(ClinikDbContext dbContext) : IClinicRepository
{
    public async Task<ClinicEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Clinics.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
    public async Task<ClinicEntity?> GetByIdWithInclude(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Clinics
            .AsTracking()
            .Include(c => c.Ownerships)
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
    public void Insert(ClinicEntity clinic)
    {
        dbContext.Clinics.Add(clinic);
    }
}
