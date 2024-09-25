using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Models;

namespace Dfe.Data.SearchPrototype.Web.Mappers
{
    /// <summary>
    /// Facilitates mapping from the facts received from the use-case search response,
    /// together with the selected facets (derived from user input), and maps into a
    /// list of <see cref="Facet"/> instances to establish the required view-model.
    /// </summary>
    public sealed class FacetsAndSelectedFacetsToFacetsViewModelMapper : IMapper<FacetsAndSelectedFacets, List<Facet>?>
    {
        /// <summary>
        /// Provides the functionality to map from user selected facets,
        /// together with the facets returned from the use-case search
        /// response, into a configured list of <see cref="Facet"/> instances.
        /// The selected facets are used to re-map the pre-established user
        /// selections on the incoming (revised) facet portion of the view-model.
        /// </summary>
        /// <param name="input">
        /// The dictionary of selected facets (user input) and <see cref="EstablishmentFacets"/>
        /// facet results returned from the use-case search.
        /// </param>
        /// <returns>
        /// The configured list of <see cref="Facet"/> instances expected.
        /// </returns>
        public List<Facet>? MapFrom(FacetsAndSelectedFacets input)
        {
            List<Facet>? facetItems = null;

            if (input.Facets != null)
            {
                facetItems = [];

                foreach (EstablishmentFacet establishmentFacet in input.Facets.Facets)
                {
                    List<FacetValue> facetValues =
                        establishmentFacet.Results.Select(
                            result =>
                                new FacetValue(
                                    Value: result.Value,
                                    Count: result.Count,
                                    IsSelected:
                                        input.SelectedFacets?.ContainsKey(establishmentFacet.Name) == true &&
                                            input.SelectedFacets[establishmentFacet.Name]
                                                .Any(selectedValue => result.Value == selectedValue))
                                )
                                .ToList();

                    facetItems.Add(new Facet(Name: establishmentFacet.Name, Values: facetValues));
                }
            }

            return facetItems;
        }
    }

    /// <summary>
    /// Encapsulates the request objects necessary to attempt a valid mapping
    /// of the required collection of <see cref="Facet"/> view models.
    /// </summary>
    public class FacetsAndSelectedFacets
    {
        /// <summary>
        /// The facets derived from the provisioned use-case search.
        /// </summary>
        public EstablishmentFacets? Facets { get; }

        /// <summary>
        /// The facets previously selected by the user.
        /// </summary>
        public Dictionary<string, List<string>>? SelectedFacets { get; }

        /// <summary>
        /// Establishes an immutable <see cref="FacetsAndSelectedFacets">
        /// instance via the constructor arguments specified.
        /// </summary>
        /// <param name="establishmentFacets">
        /// The <see cref="EstablishmentFacets"/> instance configured
        /// from the use-case search response.
        /// </param>
        /// <param name="selectedFacets">
        /// The dictionary of selected facets (user input) previously selected.
        /// </param>
        public FacetsAndSelectedFacets(
            EstablishmentFacets? establishmentFacets,
            Dictionary<string, List<string>>? selectedFacets = null)
        {
            Facets = establishmentFacets;
            SelectedFacets = selectedFacets;
        }
    }
}
