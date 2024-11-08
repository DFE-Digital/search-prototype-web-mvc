using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;

public class AngleSharpDocumentClientProvider : IDocumentQueryClientProvider
{
    private readonly HttpClient _client;

    public AngleSharpDocumentClientProvider(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    public async Task<IDocumentClient> CreateDocumentClientAsync(HttpRequestMessage httpRequestMessage)
    {
        HttpResponseMessage httpResponseMessage = await _client.SendAsync(httpRequestMessage);
        var document = await HtmlHelpers.GetDocumentAsync(httpResponseMessage);
        return new AngleSharpQueryClient(document);
    }
}
