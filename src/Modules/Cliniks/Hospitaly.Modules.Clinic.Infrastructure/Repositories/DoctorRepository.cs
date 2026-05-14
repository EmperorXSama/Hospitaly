using Hospitaly.Modules.Clinic.Domain.Doctor;
using Hospitaly.Modules.Clinic.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hospitaly.Modules.Clinic.Infrastructure.Repositories;

public class DoctorRepository(ClinikDbContext dbContext) : IDoctorRepository
{
    public async Task<Doctor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Doctors.SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<Doctor?> GetByIdWithIncludeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Doctors
            .AsTracking()
            .Include("_credentials")
            .Include("_specialties")
            .Include("_affiliations")
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public void Insert(Doctor doctor)
    {
        dbContext.Doctors.Add(doctor);
    }
}
