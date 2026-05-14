using Hospitaly.Modules.Clinic.Domain.Specialty;
using Hospitaly.Modules.Clinic.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hospitaly.Modules.Clinic.Infrastructure.Repositories;

public class SpecialtyRepository(ClinikDbContext dbContext) : ISpecialtyRepository
{
    public async Task<IEnumerable<Specialty>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Specialties.
            AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}