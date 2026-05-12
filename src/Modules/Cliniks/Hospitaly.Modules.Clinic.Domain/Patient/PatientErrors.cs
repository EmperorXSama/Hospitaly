using ErrorOr;

namespace Hospitaly.Modules.Clinic.Domain.Patient;

public static class PatientErrors
{
    public static Error NotFound(Guid patientId) =>
        Error.NotFound(
            "Patient.NotFound",
            $"The patient with identifier {patientId} was not found.");

    public static Error AlreadyRegistered() =>
        Error.Conflict(
            "Patient.AlreadyRegistered",
            "The patient is already registered.");
}
