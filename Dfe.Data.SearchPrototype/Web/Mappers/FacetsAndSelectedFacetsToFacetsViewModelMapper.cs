using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Models;

namespace Dfe.Data.SearchPrototype.Web.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FacetsAndSelectedFacetsToFacetsViewModelMapper : IMapper<FacetsAndSelectedFacets, List<Facet>?>
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">
        /// 
        /// </param>
        /// <returns>
        /// 
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
        /// 
        /// </summary>
        public EstablishmentFacets? Facets { get; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, List<string>>? SelectedFacets { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="establishmentFacets">
        /// 
        /// </param>
        /// <param name="selectedFacets">
        /// 
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
