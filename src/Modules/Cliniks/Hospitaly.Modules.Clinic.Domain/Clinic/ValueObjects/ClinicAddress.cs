using ErrorOr;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

public sealed record ClinicAddress
{
    public Address Value { get; init; }
    public Coordinates? Coordinates { get; init; }

    private ClinicAddress()
    {
    }

    private ClinicAddress(Address value, Coordinates? coordinates)
    {
        Value = value;
        Coordinates = coordinates;
    }

    public static ErrorOr<ClinicAddress> Create(
        string street,
        string city,
        string? region,
        string? postalCode,
        string country,
        Coordinates? coordinates = null)
    {
        var address = Address.Create(street, city, region, postalCode, country);
        if (address.IsError)
        {
            return address.Errors;
        }

        return new ClinicAddress(address.Value, coordinates);
    }
}
