using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;

public class AngleSharpDomQueryClientFactory : IDomQueryClientFactory
{
    private readonly HttpClient _httpClient;

    public AngleSharpDomQueryClientFactory(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<IDomQueryClient> CreateClientFromHttpRequestAsync(HttpRequestMessage httpRequestMessage)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);
        var document = await HtmlHelpers.GetDocumentAsync(httpResponseMessage);
        return new AngleSharpQueryClient(document);
    }
}
