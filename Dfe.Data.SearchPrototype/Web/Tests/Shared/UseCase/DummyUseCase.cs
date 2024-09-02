using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.UseCase;

public class DummyUseCase : IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>
{
    public Task<SearchByKeywordResponse> HandleRequest(SearchByKeywordRequest request)
    {
        var response = SearchByKeywordResponseTestDouble.Create();

        return Task.FromResult(response);
    }
}
