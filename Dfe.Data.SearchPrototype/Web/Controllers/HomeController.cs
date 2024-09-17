using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Data.SearchPrototype.Web.Controllers;

/// <summary>
/// Controller responsible for allowing searching by keyword.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> _searchByKeywordUseCase;
    private readonly IMapper<SearchByKeywordResponse, SearchResultsViewModel> _responseMapper;
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
        IMapper<SearchByKeywordResponse, SearchResultsViewModel> responseMapper,
        IMapper<Dictionary<string, List<string>>, IList<FilterRequest>> requestMapper)
    {
        _logger = logger;
        _searchByKeywordUseCase = searchByKeywordUseCase;
        _responseMapper = responseMapper;
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
                new SearchByKeywordRequest(searchKeyWord + "*"));

        return View("Index", _responseMapper.MapFrom(response));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchKeyWord"></param>
    /// <param name="viewModelResponse"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SearchWithFilters(
        string searchKeyWord, Dictionary<string, List<string>> selectedFacets)
    {
        ViewBag.SearchQuery = searchKeyWord;
        SearchByKeywordResponse response = null;

        if (selectedFacets != null && selectedFacets.Any())
        {
            IList<FilterRequest> filterRequests = _requestMapper.MapFrom(selectedFacets);

            // Mapper
            response =
                await _searchByKeywordUseCase.HandleRequest(
                    new SearchByKeywordRequest(searchKeyWord + "*", filterRequests));
        }
        else
        {
            return await Index(searchKeyWord);
        }

        return View("Index", _responseMapper.MapFrom(response));
    }
}
