namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels;

/// <summary>
/// A view model representation of an address from a single search result.
/// </summary>
public class Address
{
    /// <summary>
    /// First line of the address
    /// </summary>
    public string? Street { get; set; }
    /// <summary>
    /// Second line of the address
    /// </summary>
    public string? Locality { get; set; }
    /// <summary>
    /// Third line of the address
    /// </summary>
    public string? Address3 { get; set; }
    /// <summary>
    /// Fourth line of the address
    /// </summary>
    public string? Town { get; set; }
    /// <summary>
    /// Postcode
    /// </summary>
    public string? Postcode { get; set; }
    /// <summary>
    /// Establishment address.
    /// </summary>
    /// <returns>
    /// Address formatted as a display-friendly string
    /// </returns>
    public string AddressAsString()
    {
        var addressComponents
            = new[] { Street, Locality, Address3, Town, Postcode }
                .Where(addressComponent => !string.IsNullOrEmpty(addressComponent))
                .ToArray();
        var readableAddress = string.Join(", ", addressComponents);

        return readableAddress;
    }
}
