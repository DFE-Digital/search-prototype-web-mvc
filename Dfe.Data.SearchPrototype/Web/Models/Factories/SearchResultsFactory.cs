using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels;
using Dfe.Data.SearchPrototype.Web.Services;

namespace Dfe.Data.SearchPrototype.Web.Models.Factories
{
    /// <summary>
    /// Factory implementation to facilitate mapping from the incoming <see cref="EstablishmentResults"/> and <see cref="FacetsAndSelectedFacets"/>
    /// instances to the required <see cref="SearchResults"/> type. The mapper is responsible for taking the response from the use-case search
    /// request, together with the previously submitted user input, and amalgamates these results into the expected view-model.
    /// </summary>
    public sealed class SearchResultsFactory : ISearchResultsFactory
    {
        private readonly IMapper<EstablishmentResults?, List<ViewModels.Establishment>?> _establishmentResultsToEstablishmentsViewModelMapper;
        private readonly IMapper<FacetsAndSelectedFacets, List<Facet>?> _facetsAndSelectedFacetsToFacetsViewModelMapper;
        private INameKeyToDisplayNameProvider _displayNamesProvider;

        /// <summary>
        /// The following dependencies are mappers which facilitate the transformation from the use-case response to the required view model.
        /// </summary>
        /// <param name="establishmentResultsToEstablishmentsViewModelMapper">
        /// The concrete implementation of the T:DfE.Data.ComponentLibrary.CrossCuttingConcerns.Mapping.IMapper<EstablishmentResults, SearchByKeywordResponse>
        /// defined within, and injected by the IOC container (defined within program.cs)
        /// </param>
        /// <param name="facetsAndSelectedFacetsToFacetsViewModelMapper">
        /// The concrete implementation of the T:DfE.Data.ComponentLibrary.CrossCuttingConcerns.Mapping.IMapper<FacetsAndSelectedFacets, List<Facet>?>
        /// defined within, and injected by the IOC container (defined within program.cs) used to map all facets and pre-selections from the response to the view model.
        /// </param>
        public SearchResultsFactory(
            IMapper<EstablishmentResults?, List<ViewModels.Establishment>?> establishmentResultsToEstablishmentsViewModelMapper,
            IMapper<FacetsAndSelectedFacets, List<Facet>?> facetsAndSelectedFacetsToFacetsViewModelMapper,
            INameKeyToDisplayNameProvider displayNamesProvider)
        {
            _establishmentResultsToEstablishmentsViewModelMapper = establishmentResultsToEstablishmentsViewModelMapper;
            _facetsAndSelectedFacetsToFacetsViewModelMapper = facetsAndSelectedFacetsToFacetsViewModelMapper;
            _displayNamesProvider = displayNamesProvider;
        }

        /// <summary>
        /// Factory method for creating the required <see cref="ViewModels.SearchResults"/> view model
        /// based on the service response and the previously selected (if any) facets which are
        /// reinstated via the mappings provisioned. We should only allow facets to be configured
        /// if they are accompanied by configured establishments under the <see cref="EstablishmentResults"/>
        /// search response object. If we have nothing to map, a default (empty) <see cref="SearchResults"/>
        /// instance is returned.
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
            FacetsAndSelectedFacets facetsAndSelectedFacets) =>
                (establishmentResults?.Establishments.Count > 0) ?
                   new(_displayNamesProvider)
                   {
                        SearchItems =
                            _establishmentResultsToEstablishmentsViewModelMapper
                                .MapFrom(establishmentResults),
                        Facets =
                            _facetsAndSelectedFacetsToFacetsViewModelMapper
                                .MapFrom(facetsAndSelectedFacets)
                   }
                   : new(_displayNamesProvider); // default.
    }
}
