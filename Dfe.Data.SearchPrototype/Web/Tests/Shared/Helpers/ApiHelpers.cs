using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using System.Text.Json.Serialization;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;

public sealed class ApiHelpers
{
    public class EstablishmentResultsProperty
    {
        [JsonPropertyName("establishmentResults")]
        public EstablishmentSearchResults? EstablishmentResults { get; set; }
    }

    public class EstablishmentSearchResults
    {
        [JsonPropertyName("establishments")]
        public IEnumerable<Establishment>? Establishments { get; set; }
    }
}
