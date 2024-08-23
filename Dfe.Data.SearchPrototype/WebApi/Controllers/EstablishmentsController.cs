using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Data.SearchPrototype.WebApi.Controllers
{
    [ApiController]
    [Route("establishments")]
    public class EstablishmentsController : ControllerBase
    {
        private readonly ILogger<EstablishmentsController> _logger;
        private readonly IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> _searchByKeywordUseCase;

        public EstablishmentsController(
            ILogger<EstablishmentsController> logger,
            IUseCase<SearchByKeywordRequest, SearchByKeywordResponse> searchByKeywordUseCase)
        {
            _logger = logger;
            _searchByKeywordUseCase = searchByKeywordUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetEstablishments([FromQuery] EstablishmentsRequest request)
        {
            var searchByKeywordRequest = new SearchByKeywordRequest(request.SearchKeyword, "establishments");
            var response = await _searchByKeywordUseCase.HandleRequest(searchByKeywordRequest);

            return Ok(response);
        }
    }
}
