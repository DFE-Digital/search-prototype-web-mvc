namespace Dfe.Data.SearchPrototype.Web.ViewModels;

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
}
