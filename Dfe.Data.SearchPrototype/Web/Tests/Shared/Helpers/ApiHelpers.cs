using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels;
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
        public IEnumerable<SearchForEstablishments.Models.Establishment>? Establishments { get; set; }
    }

    public class EstablishmentFacetsProperty
    {
        [JsonPropertyName("establishmentFacetResults")]
        public EstablishmentFacets? EstablishmentFacetResults { get; set; }
    }

    public class EstablishmentFacets
    {
        [JsonPropertyName("facets")]
        public List<EstablishmentFacet>? Facets { get; set; }
    }

    public class EstablishmentFacet
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
