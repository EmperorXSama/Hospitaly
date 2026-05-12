using System.Reflection;

namespace Hospitaly.Modules.Clinic.Infrastructure;

public static  class AssemblyReference
{
    public static readonly Assembly assembly = typeof(AssemblyReference).Assembly;
}