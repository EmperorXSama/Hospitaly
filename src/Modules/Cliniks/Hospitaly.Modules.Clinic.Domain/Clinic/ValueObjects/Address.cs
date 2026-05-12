using ErrorOr;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

public sealed record Address
{
    public string Street { get; init; }
    public string City { get; init; }
    public string? Region { get; init; }
    public string? PostalCode { get; init; }
    public string Country { get; init; }

    private Address()
    {
    }

    private Address(
        string street,
        string city,
        string? region,
        string? postalCode,
        string country)
    {
        Street = street;
        City = city;
        Region = region;
        PostalCode = postalCode;
        Country = country;
    }

    public static ErrorOr<Address> Create(
        string street,
        string city,
        string? region,
        string? postalCode,
        string country)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(street))
        {
            errors.Add(Error.Validation(
                code: "Address.InvalidStreet",
                description: "Street cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["street"] = street }));
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            errors.Add(Error.Validation(
                code: "Address.InvalidCity",
                description: "City cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["city"] = city }));
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            errors.Add(Error.Validation(
                code: "Address.InvalidCountry",
                description: "Country cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["country"] = country }));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Address(street, city, region, postalCode, country);
    }
}
