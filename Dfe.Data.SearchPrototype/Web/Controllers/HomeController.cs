using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Data.SearchPrototype.Web.Controllers;

/// <summary>
/// Controller responsible for allowing searching by keyword.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> _searchByKeywordUseCase;
    private readonly IMapper<EstablishmentResults?, List<ViewModels.Establishment>?> _establishmentResultsToEstablishmentsViewModelMapper;
    private readonly IMapper<EstablishmentFacetsMapperRequest, List<Facet>?> _establishmentFacetsToFacetsViewModelMapper;
    private readonly IMapper<Dictionary<string, List<string>>, IList<FilterRequest>> _requestMapper;


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
    /// <param name="mapper">
    /// The concrete implementation of the T:DfE.Data.ComponentLibrary.CrossCuttingConcerns.Mapping.IMapper<EstablishmentResults, SearchByKeywordResponse>
    /// defined within, and injected by the IOC container (defined within program.cs)
    /// </param>
    public HomeController(
        ILogger<HomeController> logger,
        IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> searchByKeywordUseCase,
        IMapper<EstablishmentResults?, List<ViewModels.Establishment>?> establishmentResultsToEstablishmentsViewModelMapper,
        IMapper<EstablishmentFacetsMapperRequest, List<Facet>?> establishmentFacetsToFacetsViewModelMapper,
        IMapper<Dictionary<string, List<string>>, IList<FilterRequest>> requestMapper)
    {
        _logger = logger;
        _searchByKeywordUseCase = searchByKeywordUseCase;
        _establishmentResultsToEstablishmentsViewModelMapper = establishmentResultsToEstablishmentsViewModelMapper;
        _establishmentFacetsToFacetsViewModelMapper = establishmentFacetsToFacetsViewModelMapper;
        _requestMapper = requestMapper;
    }

    /// <summary>
    /// The action method that composes the view model based on the search keyword.
    /// </summary>
    /// <param name="searchKeyWord">search keyword</param>
    /// <returns>
    /// An IActionResult contract that represents the result of this action method.
    /// </returns>
    public async Task<IActionResult> Index(string searchKeyWord)
    {
        if (string.IsNullOrEmpty(searchKeyWord))
        {
            return View();
        }

        ViewBag.SearchQuery = searchKeyWord;

        SearchByKeywordResponse response =
            await _searchByKeywordUseCase.HandleRequest(
                new SearchByKeywordRequest(searchKeyword: searchKeyWord + "*"));

        ViewModels.SearchResults viewModel =
               CreateViewModel(
                   response.EstablishmentResults,
                   new EstablishmentFacetsMapperRequest(response.EstablishmentFacetResults));

        return View("Index", viewModel);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchRequestViewModel">
    /// 
    /// </param>
    /// <returns>
    /// 
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> SearchWithFilters(SearchRequest searchRequestViewModel)
    {
        ViewBag.SearchQuery = searchRequestViewModel.SearchKeyword;

        if (searchRequestViewModel.HasSearchKeyWord &&
            searchRequestViewModel.HasSelectedFacets)
        {
            SearchByKeywordResponse response =
                await _searchByKeywordUseCase.HandleRequest(
                    new SearchByKeywordRequest(
                        searchKeyword: searchRequestViewModel.SearchKeyword + "*",
                        filterRequests: _requestMapper.MapFrom(searchRequestViewModel.SelectedFacets!)));

            ViewModels.SearchResults viewModel =
                CreateViewModel(
                    response.EstablishmentResults,
                    new EstablishmentFacetsMapperRequest(
                        response.EstablishmentFacetResults, searchRequestViewModel.SelectedFacets));

            return View("Index", viewModel);
        }

        return await Index(searchRequestViewModel.SearchKeyword!);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="establishmentResults">
    /// 
    /// </param>
    /// <param name="facetMapperRequest">
    /// 
    /// </param>
    /// <returns>
    /// 
    /// </returns>
    private ViewModels.SearchResults CreateViewModel(
        EstablishmentResults? establishmentResults,
        EstablishmentFacetsMapperRequest facetMapperRequest) => new(){
            SearchItems = _establishmentResultsToEstablishmentsViewModelMapper.MapFrom(establishmentResults),
            Facets = _establishmentFacetsToFacetsViewModelMapper.MapFrom(facetMapperRequest)
        };
}
