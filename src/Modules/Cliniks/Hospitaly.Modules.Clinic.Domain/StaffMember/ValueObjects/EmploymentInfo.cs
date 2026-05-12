using ErrorOr;
using Hospitaly.Modules.Clinic.Domain.StaffMember.Enums;

namespace Hospitaly.Modules.Clinic.Domain.StaffMember.ValueObjects;

public sealed record EmploymentInfo
{
    public DateTime HireDate { get; }
    public EmploymentStatus Status { get; }
    public ContractType ContractType { get; }

    private EmploymentInfo()
    {
    }

    private EmploymentInfo(DateTime hireDate, EmploymentStatus status, ContractType contractType)
    {
        HireDate = hireDate;
        Status = status;
        ContractType = contractType;
    }

    public static ErrorOr<EmploymentInfo> Create(DateTime hireDate, EmploymentStatus status, ContractType contractType)
    {
        if (hireDate > DateTime.UtcNow)
        {
            return Error.Validation(
                "EmploymentInfo.FutureHireDate",
                "Hire date cannot be in the future.");
        }

        return new EmploymentInfo(hireDate, status, contractType);
    }

    public override string ToString() => $"{Status} {ContractType} (hired {HireDate:yyyy-MM-dd})";
}
