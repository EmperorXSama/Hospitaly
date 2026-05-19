namespace Hospitaly.Modules.Users.Domain.Users;

public sealed class Role
{

    public static readonly Role Member = new Role(nameof(Member));
    public static readonly Role Administrator = new Role(nameof(Administrator));
    public static readonly Role Doctor = new Role(nameof(Doctor));
    public static readonly Role Nurse = new Role(nameof(Nurse));
    public static readonly Role Pharmacist = new Role(nameof(Pharmacist));
    public static readonly Role HospitalAdministrator = new Role(nameof(HospitalAdministrator));
    public static readonly Role Patient = new Role(nameof(Patient));
        
    private static IReadOnlyDictionary<string, Role> _roles = new Dictionary<string, Role>()
    {
        { Member.Name, Member },
        { Administrator.Name, Administrator },
        { Doctor.Name, Doctor },
        { Nurse.Name, Nurse },
        { Pharmacist.Name, Pharmacist },
        { HospitalAdministrator.Name, HospitalAdministrator },
        { Patient.Name, Patient }
    };

    private Role(string name)
    {
        Name = name;
    }
    private Role(){}
    public string Name { get; private set; }

    public static bool IsValid(string roleName)
    {
        return _roles.ContainsKey(roleName);
    }

    public static bool GetFromName(string roleName, out Role? role)
    {
        role = null;
        
        return _roles.TryGetValue(roleName, out role);
    }
}