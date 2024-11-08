using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages;

public interface IPageFactory
{
    public Task<TPage> CreatePageAsync<TPage>(HttpRequestMessage httpRequest) where TPage : BasePage, new();
}

public sealed class PageFactory : IPageFactory
{
    private readonly IDocumentQueryClientProvider _documentClientFactory;

    public PageFactory(IDocumentQueryClientProvider documentClientFactory)
    {
        _documentClientFactory = documentClientFactory;
    }
    public async Task<TPage> CreatePageAsync<TPage>(HttpRequestMessage httpRequestMessage) where TPage : BasePage, new()
        => new TPage()
        {
            DocumentClient = await _documentClientFactory.CreateDocumentClientAsync(httpRequestMessage)
        };
}

