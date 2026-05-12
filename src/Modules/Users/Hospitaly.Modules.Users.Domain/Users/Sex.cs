using ErrorOr;

namespace Hospitaly.Modules.Users.Domain.Users;

public sealed class Sex
{
    public static readonly Sex Male = new("Male");
    public static readonly Sex Female = new("Female");
    public static readonly Sex Other = new("Other");

    public static readonly IReadOnlyCollection<Sex> All = [Male, Female, Other];

    public static ErrorOr<Sex> FromName(string name)
    {
        var sex = All.FirstOrDefault(s => s.Name == name);
        return sex is not null
            ? sex
            : Error.Validation("Sex.Invalid", $"'{name}' is not a valid sex");
    }

    private Sex(string name) => Name = name;
    private Sex() { }
    public string Name { get; private set; }

    public override string ToString() => Name;
}
