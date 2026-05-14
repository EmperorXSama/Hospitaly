using Hospitaly.Modules.Clinic.Application.Specielties.Queries.GetSpecialties;
using Hospitaly.Modules.Clinic.Domain.Specialty;

namespace Hospitaly.Modules.Clinic.Application.Mappers;


internal class SpecialtyNode
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public List<SpecialtyNode> Children { get; } = [];

    public SpecialtyResponse ToResponse() =>
        new(Id, Name, Children.Select(c => c.ToResponse()).ToList());
}

public static class SpecialtyMapper
{
    public static SpecialtiesResponse ToHierarchy(IEnumerable<Specialty> flatList)
    {
        var lookup = flatList.ToDictionary(
            s => s.Id,
            s => new SpecialtyNode { Id = s.Id, Name = s.Name }
        );

        var roots = new List<SpecialtyNode>();

        foreach (var specialty in flatList)
        {
            if (specialty.ParentId is null)
                roots.Add(lookup[specialty.Id]);
            else if (lookup.TryGetValue(specialty.ParentId.Value, out var parent))
                parent.Children.Add(lookup[specialty.Id]); // ✅ no cast needed
        }

      
        return new SpecialtiesResponse(roots.Select(r => r.ToResponse()).ToList());
    }
}