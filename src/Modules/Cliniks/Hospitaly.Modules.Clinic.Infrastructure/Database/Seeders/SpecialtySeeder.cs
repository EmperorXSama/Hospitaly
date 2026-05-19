using Hospitaly.Common.Infrastructure.Seeder;
using Hospitaly.Modules.Clinic.Domain.Specialty;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Seeders;

public class SpecialtySeeder(ClinikDbContext dbContext, ILogger<SpecialtySeeder> logger) : ISeeder
{
    public int Order => 1;
    public bool IsOptional=>false;
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
       var doSpecialtyTableHasAny = await dbContext.Specialties.AnyAsync(cancellationToken);
       if (doSpecialtyTableHasAny)
       {
           logger.LogInformation("seeding Specialties data passed because data is already seeded");
           return;
       }

       await dbContext.Specialties.AddRangeAsync(_specialties, cancellationToken);
       
       await dbContext.SaveChangesAsync(cancellationToken);
       
    }

    public async Task ValidateAsync(CancellationToken cancellationToken = default)
    {
        if (IsOptional)
        {
            return;
        }
        
        bool hasData = await dbContext.Specialties.AnyAsync(cancellationToken);
        if (!hasData)
        {
            throw new InvalidOperationException(
                $"Required seed data is missing for '{nameof(SpecialtySeeder)}'. " +
                $"Run the application with --seed to populate required data.");
        }
    }
    
    private static readonly List<Specialty> _specialties = CreateSpecialties();
    private static List<Specialty> CreateSpecialties()
    {
        // Root specialties
        var general = Specialty.Create("Médecine Générale");

        var medical = Specialty.Create("Spécialités Médicales");
        var surgical = Specialty.Create("Spécialités Chirurgicales");

        var gyneco = Specialty.Create("Gynécologie & Obstétrique");
        var pediatrics = Specialty.Create("Pédiatrie");
        var radiology = Specialty.Create("Radiologie");
        var biology = Specialty.Create("Biologie Médicale");

        // Medical specialties
        var cardiology = Specialty.Create("Cardiologie", medical.Id);
        var dermatology = Specialty.Create("Dermatologie", medical.Id);
        var endocrinology = Specialty.Create("Endocrinologie", medical.Id);
        var gastro = Specialty.Create("Gastro-entérologie", medical.Id);
        var neurology = Specialty.Create("Neurologie", medical.Id);

        // Surgical specialties
        var generalSurgery = Specialty.Create("Chirurgie Générale", surgical.Id);
        var orthopedic = Specialty.Create("Orthopédie", surgical.Id);
        var neurosurgery = Specialty.Create("Neurochirurgie", surgical.Id);
        var urology = Specialty.Create("Urologie", surgical.Id);
        var ophthalmology = Specialty.Create("Ophtalmologie", surgical.Id);

        return new List<Specialty>
        {
            // Roots
            general,
            medical,
            surgical,
            gyneco,
            pediatrics,
            radiology,
            biology,

            // Medical children
            cardiology,
            dermatology,
            endocrinology,
            gastro,
            neurology,

            // Surgical children
            generalSurgery,
            orthopedic,
            neurosurgery,
            urology,
            ophthalmology
        };
    }
}