using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Domain.Specialty;

namespace Hospitaly.Modules.Clinic.Application.Specielties.Queries.GetSpecialties;

public sealed record GetSpecialtiesQuery:IQuery<SpecialtiesResponse>;

public record SpecialtyResponse(
    Guid Id,
    string Name,
    IReadOnlyList<SpecialtyResponse> Children
);

public record SpecialtiesResponse(
    IReadOnlyList<SpecialtyResponse> Specialties
);