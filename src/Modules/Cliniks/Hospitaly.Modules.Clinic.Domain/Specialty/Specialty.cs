using Hospitaly.Common.Domain;

namespace Hospitaly.Modules.Clinic.Domain.Specialty;

public class Specialty : Entity
{
    public string Name { get; private set; }
    public Guid? ParentId { get; set; }
    
    
    public Specialty? Parent { get; set; }
    
    public ICollection<Specialty> Children { get; set; } = [];
    
    private Specialty() { }
    private Specialty(AuditInfo auditInfo) : base(auditInfo,Guid.NewGuid()) { }
    
    public static Specialty Create(string name, Guid? parentId = null)
    {
        return new Specialty(new AuditInfo(Guid.Empty, DateTime.UtcNow))
        {
            
            Id = Guid.NewGuid(),
            Name = name,
            ParentId = parentId
        };
    }
}