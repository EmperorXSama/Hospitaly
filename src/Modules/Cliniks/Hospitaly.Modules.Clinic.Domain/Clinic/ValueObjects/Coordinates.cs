using ErrorOr;
using System.Collections.Generic;
using System.Linq;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

public sealed record Coordinates
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }

    private Coordinates()
    {
    }

    private Coordinates(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public static ErrorOr<Coordinates> Create(double latitude, double longitude)
    {
        var errors = new List<Error>();

        if (latitude < -90 || latitude > 90)
        {
            errors.Add(Error.Validation(
                code: "Coordinates.InvalidLatitude",
                description: "Latitude must be between -90 and 90.",
                metadata: new Dictionary<string, object> { ["latitude"] = latitude }));
        }

        if (longitude < -180 || longitude > 180)
        {
            errors.Add(Error.Validation(
                code: "Coordinates.InvalidLongitude",
                description: "Longitude must be between -180 and 180.",
                metadata: new Dictionary<string, object> { ["longitude"] = longitude }));
        }

        if (errors.Any())
        {
            return errors;
        }

        return new Coordinates(latitude, longitude);
    }
}
