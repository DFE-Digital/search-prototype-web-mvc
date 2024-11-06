using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;

public class AngleSharpDocumentClientFactory : IDocumentClientFactory
{
    private readonly HttpClient _client;

    public AngleSharpDocumentClientFactory(HttpClient client)
    {
        _client = client;
    }
    public async Task<IDocumentClient> CreateDocumentClientAsync(HttpRequestMessage httpRequestMessage)
    {
        HttpResponseMessage httpResponseMessage = await _client.SendAsync(httpRequestMessage);
        var document = await HtmlHelpers.GetDocumentAsync(httpResponseMessage);
        return new AngleSharpQueryClient(document);
    }
}
