using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Infrastructure;
using Hospitaly.Modules.Users.Application.Abstractions.Data;
using Hospitaly.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Hospitaly.Modules.Users.Infrastructure.Database;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options) , IUnitOfWork
{

    internal DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.HasDefaultSchema(Schemas.Users);

        modelBuilder.ApplyConfigurationsFromAssembly(ReferenceAssembly.assembly);
    }
}