namespace Dfe.Data.SearchPrototype.Web.ViewModels;

/// <summary>
/// A view model representation of a single search result.
/// </summary>
public class Establishment
{
    /// <summary>
    /// Establishment Urn.
    /// </summary>
    public string Urn { get; init; } = string.Empty;
    /// <summary>
    /// Establishment name.
    /// </summary>
    public string Name { get; init; } = string.Empty;
    /// <summary>
    /// Establishment address.
    /// </summary>
    public Address Address { get; init; } = new();
    /// <summary>
    /// Establishment type.
    /// </summary>
    public string EstablishmentType {  get; init; } = string.Empty;
    /// <summary>
    /// Establishment education phase
    /// </summary>
    public string PhaseOfEducation { get; init; } = string.Empty;
    /// <summary>
    /// Establishment status code.
    /// </summary>
    public string EstablishmentStatusName { get; init; } = string.Empty;
    /// <summary>
    /// Establishment address.
    /// </summary>
    /// <returns>
    /// Address formatted as a display-friendly string
    /// </returns>
    public string AddressAsString()
    {
        var addressComponents
            = new[] { Address.Street, Address.Locality, Address.Address3, Address.Town, Address.Postcode }
                .Where(addressComponent => !string.IsNullOrEmpty(addressComponent))
                .ToArray();
        var readableAddress = string.Join(", ", addressComponents);

        return readableAddress;
    }
}