using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.ViewModels;

namespace Dfe.Data.SearchPrototype.Web.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EstablishmentFacetsToFacetsViewModelMapper : IMapper<EstablishmentFacetsMapperRequest, List<Facet>?>
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
        public List<Facet>? MapFrom(EstablishmentFacetsMapperRequest input)
        {
            List<Facet>? facetItems = null;

            if (input.EstablishmentFacets != null)
            {
                facetItems = [];

                foreach (EstablishmentFacet establishmentFacet in input.EstablishmentFacets.Facets)
                {
                    List<FacetValue> facetValues =
                        establishmentFacet.Results.Select(
                            result =>
                                new FacetValue(
                                    Value: result.Value,
                                    Count: result.Count,
                                    IsSelected:
                                        input.SelectedFacets?.ContainsKey(establishmentFacet.Name) == true &&
                                            input.SelectedFacets[establishmentFacet.Name].Any(selectedValue => result.Value == selectedValue))
                                )
                                .ToList();

                    facetItems.Add(new Facet(Name: establishmentFacet.Name, Values: facetValues));
                }
            }

            return facetItems;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EstablishmentFacetsMapperRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public EstablishmentFacets? EstablishmentFacets { get; }

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
        public EstablishmentFacetsMapperRequest(EstablishmentFacets? establishmentFacets, Dictionary<string, List<string>>? selectedFacets = null)
        {
            EstablishmentFacets = establishmentFacets;
            SelectedFacets = selectedFacets;
        }
    }
}
