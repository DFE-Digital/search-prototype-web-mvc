using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;

namespace Dfe.Data.SearchPrototype.Web.Models.Factories
{
    /// <summary>
    /// Describes the behaviour required to facilitate mapping from the incoming <see cref="EstablishmentResults"/>
    /// and <see cref="FacetsAndSelectedFacets"/> instances to the required <see cref="SearchResults"/> type.
    /// </summary>
    public interface ISearchResultsFactory
    {
        /// <summary>
        /// Describes the behaviour for the factory method used for creating the required <see cref="SearchResults"/>
        /// view model based on the service response and the previously selected (if any) facets which are
        /// reinstated via the mappings provisioned.
        /// </summary>
        /// <param name="establishmentResults">
        /// Encapsulates the aggregation of <see cref="SearchForEstablishments.Models.Establishment" />
        /// types returned from the underlying search system.
        /// </param>
        /// <param name="facetsAndSelectedFacets">
        /// Encapsulates the request objects necessary to attempt a valid mapping
        /// of the required collection of <see cref="Facet"/> view models.
        /// </param>
        /// <returns>
        /// The <see cref="ViewModels.SearchResults"/> generated as a result of combining the
        /// establishment result and facet result mappers.
        /// </returns>
        public ViewModels.SearchResults CreateViewModel(
            EstablishmentResults? establishmentResults,
            FacetsAndSelectedFacets facetsAndSelectedFacets);
    }
}