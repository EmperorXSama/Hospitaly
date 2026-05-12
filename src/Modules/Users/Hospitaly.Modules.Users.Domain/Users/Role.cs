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


    private Role(string name)
    {
        Name = name;
    }
    private Role(){}
    public string Name { get; private set; }
}