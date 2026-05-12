namespace Hospitaly.Modules.Users.Domain.Users;

public sealed class Permission
{
    public static readonly Permission BrowseClinic = new("clinics:read");
    public static readonly Permission ModifyUser = new("user:modify");
    public static readonly Permission GetUser = new("user:read");

    private Permission (){}
    private Permission(string code)
    {
        Code = code;
    }

    public string Code { get; private set; }
}