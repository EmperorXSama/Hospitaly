using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Application.Mappers;
using Hospitaly.Modules.Clinic.Domain.Specialty;

namespace Hospitaly.Modules.Clinic.Application.Specielties.Queries.GetSpecialties;

public class GetSpecialtiesQueryHandler : IQueryHandler<GetSpecialtiesQuery, SpecialtiesResponse>
{
    private readonly ISpecialtyRepository _repository;

    public GetSpecialtiesQueryHandler(ISpecialtyRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<SpecialtiesResponse>> Handle(GetSpecialtiesQuery request, CancellationToken cancellationToken)
    {
        var specialties = await _repository.GetAllAsync(cancellationToken);
        return SpecialtyMapper.ToHierarchy(specialties);
    }
}