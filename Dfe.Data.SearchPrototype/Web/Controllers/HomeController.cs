using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models;
using Dfe.Data.SearchPrototype.Web.Models.Factories;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Data.SearchPrototype.Web.Controllers;

/// <summary>
/// Controller responsible for allowing searching by keyword.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> _searchByKeywordUseCase;
    private readonly ISearchResultsFactory _searchResultsFactory;
    private readonly IMapper<Dictionary<string, List<string>>?, IList<FilterRequest>> _selectedFacetsToFilterRequestsMapper;

    /// <summary>
    /// The following dependencies include the use-case which orchestrates the search functionality,
    /// and the mapper which transforms from the use-case response to the view model.
    /// </summary>
    /// <param name="logger">
    /// The concrete implementation of the T:Microsoft.Extensions.Logging.ILogger
    /// defined within, and injected by the IOC container (defined within program.cs)
    /// </param>
    /// <param name="searchByKeywordUseCase">
    /// The concrete implementation of the T:DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase.IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>
    /// defined within, and injected by the IOC container (defined within program.cs)
    /// </param>
    /// <param name="searchResultsFactory">
    ///  The concrete implementation of the <see cref="ISearchResultsFactory"/> type used to create a configured instance of an <see cref="SearchResults"/> instance.
    /// </param>
    /// <param name="selectedFacetsToFilterRequestsMapper">
    /// The concrete implementation of the T:DfE.Data.ComponentLibrary.CrossCuttingConcerns.Mapping.IMapper<Dictionary<string, List<string>>, IList<FilterRequest>>
    /// defined within, and injected by the IOC container (defined within program.cs) used to map the user input to a list of <see cref="FilterRequest"/> request types.
    /// </param>
    public HomeController(
        ILogger<HomeController> logger,
        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> searchByKeywordUseCase,
        ISearchResultsFactory searchResultsFactory,
        IMapper<Dictionary<string, List<string>>?, IList<FilterRequest>> selectedFacetsToFilterRequestsMapper)
    {
        _logger = logger;
        _searchByKeywordUseCase = searchByKeywordUseCase;
        _searchResultsFactory = searchResultsFactory;
        _selectedFacetsToFilterRequestsMapper = selectedFacetsToFilterRequestsMapper;
    }

    /// <summary>
    /// The action method that composes the view model based on the <see cref="SearchRequest"/> posted from for apply
    /// filters submission which includes the search keyword and selected facet values used to apply filtering.
    /// </summary>
    /// <param name="searchRequestViewModel">
    /// Encapsulates the search keyword and selected facets used to apply filtering.
    /// </param>
    /// <returns>
    /// An IActionResult contract that represents the result of this action method.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> Index(SearchRequest searchRequestViewModel)
    {
        // TODO: add OFFSET, and LIMIT

        if (string.IsNullOrEmpty(searchRequestViewModel.SearchKeyword))
        {
            return View();
        }

        ViewBag.SearchQuery = searchRequestViewModel.SearchKeyword;

        SearchByKeywordResponse response =
            await _searchByKeywordUseCase.HandleRequest(
                new SearchByKeywordRequest(
                    searchKeyword: searchRequestViewModel.SearchKeyword!,
                    filterRequests: _selectedFacetsToFilterRequestsMapper.MapFrom(searchRequestViewModel.SelectedFacets)));

        SearchResults viewModel =
            _searchResultsFactory.CreateViewModel(
                response.EstablishmentResults,
                new FacetsAndSelectedFacets(
                    response.EstablishmentFacetResults, searchRequestViewModel.SelectedFacets));

        //viewModel.PaginationResults = new Models.ViewModels.Shared.Pagination()
        //{
        //    CurrentPageNumber = searchRequestViewModel.PageNumber.Value,  // set from view model on binding
        //    TotalPageCount = 5,     // calculated
        //    TotalRecordCount = 92,
        //    PageSequences = new Dictionary<int, int[]>()
        //    {
        //        { 1, [1,2,3,4,5] },
        //        { 2, [6,7,8,9,10] },
        //        { 3, [11,12,13,14,15] },
        //    },
        //    PageSize = 20,
        //    PageSequenceWidth = 5,
        //};

        return View(viewModel);
    }
}
