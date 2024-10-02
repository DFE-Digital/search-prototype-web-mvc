using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Tests.PartialIntegration.TestDoubles;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles
{
    public static class FacetsAndSelectedFacetsTestDouble
    {
        public static FacetsAndSelectedFacets Create() =>
            new FacetsAndSelectedFacets(
                establishmentFacets: EstablishmentFacetsTestDouble.Create(),
                selectedFacets: SelectedFacetsTestDouble.Create());

        public static FacetsAndSelectedFacets CreateWith(
            EstablishmentFacets establishmentFacets,
            Dictionary<string, List<string>> selectedFacets) =>
            new FacetsAndSelectedFacets(
                establishmentFacets: establishmentFacets,
                selectedFacets: SelectedFacetsTestDouble.Create());

        public static FacetsAndSelectedFacets CreateWithNoResults() =>
             new(
                establishmentFacets: EstablishmentFacetsTestDouble.CreateWithNoResults(),
                selectedFacets: SelectedFacetsTestDouble.Create());
    }
}
