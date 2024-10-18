namespace Dfe.Data.SearchPrototype.WebApi.Controllers;

/// <summary>
/// The search requests defining model bound view response logic predicated on the status of user input.
/// </summary>
public sealed class SearchRequest
{
    /// <summary>
    /// The search keyword provisioned by binding to user input.
    /// </summary>
    public string? SearchKeyword { get; set; }
    /// <summary>
    /// The list of values of Phase of education to filter by
    /// </summary>
    public IEnumerable<string>? PhaseOfEducation { get; set; }
    /// <summary>
    /// The list of values of Establishment status to filter by
    /// </summary>
    public IEnumerable<string>? EstablishmentStatus { get; set; }
}
