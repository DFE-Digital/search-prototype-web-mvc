using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Data.SearchPrototype.WebApi.Controllers
{
    [ApiController]
    [Route("establishments")]
    public class EstablishmentsController : ControllerBase
    {
        private readonly ILogger<EstablishmentsController> _logger;
        private readonly IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> _searchByKeywordUseCase;
        private readonly IMapper<SearchRequest, IList<FilterRequest>> _searchRequestToFilterRequestsMapper;

        public EstablishmentsController(
            ILogger<EstablishmentsController> logger,
            IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> searchByKeywordUseCase,
            IMapper<SearchRequest, IList<FilterRequest>> selectedFacetsToFilterRequestsMapper)
        {
            _logger = logger;
            _searchByKeywordUseCase = searchByKeywordUseCase;
            _searchRequestToFilterRequestsMapper = selectedFacetsToFilterRequestsMapper;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetEstablishments([FromQuery] string searchKeyword)
        //{
        //    if (string.IsNullOrEmpty(searchKeyword))
        //    {
        //        return BadRequest();
        //    }

        //    SearchByKeywordResponse response =
        //        await _searchByKeywordUseCase.HandleRequest(new SearchByKeywordRequest(
        //                searchKeyword: searchKeyword));

        //    return Ok(response);
        //}

        [HttpGet]
        public async Task<IActionResult> GetEstablishments([FromQuery] SearchRequest searchRequest)
        {
            if (string.IsNullOrEmpty(searchRequest.SearchKeyword))
            {
                return BadRequest();
            }

            SearchByKeywordResponse response =
                await _searchByKeywordUseCase.HandleRequest(new SearchByKeywordRequest(
                        searchKeyword: searchRequest.SearchKeyword,
                        filterRequests: _searchRequestToFilterRequestsMapper.MapFrom(searchRequest)
                        ));

            return Ok(response);
        }
    }
}
