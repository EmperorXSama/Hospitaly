using Hospitaly.Common.Domain;

namespace Hospitaly.Modules.Clinic.Domain.Specialty;

public class Specialty : Entity
{
    public string Name { get; private set; }
    public Guid? ParentId { get; set; }
    
    
    public Specialty? Parent { get; set; }
    
    public ICollection<Specialty> Children { get; set; } = [];
    
    private Specialty() { }
    
    public static Specialty Create(string name, Guid? parentId = null)
    {
        return new Specialty
        {
            Id = Guid.NewGuid(),
            Name = name,
            ParentId = parentId
        };
    }
}