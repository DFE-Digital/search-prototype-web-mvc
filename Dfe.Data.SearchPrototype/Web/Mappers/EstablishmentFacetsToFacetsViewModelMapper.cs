using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.ViewModels;

namespace Dfe.Data.SearchPrototype.Web.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EstablishmentFacetsToFacetsViewModelMapper : IMapper<(EstablishmentFacets?, Dictionary<string, List<string>>?), List<Facet>?>
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
        public List<Facet>? MapFrom((EstablishmentFacets?, Dictionary<string, List<string>>?) input)
        {
            List<Facet>? facetItems = null;

            if (input.Item1?.Facets != null)
            {
                facetItems = [];

                foreach (EstablishmentFacet facet in input.Item1.Facets)
                {
                    List<FacetValue> facetValues =
                        facet.Results.Select(
                            result =>
                                new FacetValue(
                                    Value: result.Value,
                                    Count: result.Count,
                                    IsSelected:
                                        input.Item2?.ContainsKey(facet.Name) == true &&
                                            input.Item2[facet.Name].Any(selectedValue => result.Value == selectedValue))
                                )
                                .ToList();

                    facetItems.Add(new Facet(Name: facet.Name, Values: facetValues));
                }
            }

            return facetItems;
        }
    }
}
