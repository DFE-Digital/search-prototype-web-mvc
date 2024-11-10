using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages;

public interface IPageFactory
{
    public Task<TPage> CreatePageAsync<TPage>(HttpRequestMessage httpRequest) where TPage : class;
}

public sealed class PageFactory : IPageFactory
{
    private readonly IServiceProvider _provider;
    private readonly IDocumentQueryClientProvider _documentQueryClientProvider;

    public PageFactory(
        IServiceProvider provider, 
        IDocumentQueryClientProvider documentClientFactory)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(documentClientFactory);
        _provider = provider;
        _documentQueryClientProvider = documentClientFactory;
    }

    public async Task<TPage> CreatePageAsync<TPage>(HttpRequestMessage httpRequestMessage) where TPage : class
    {
        IDocumentQueryClient documentClient = await _documentQueryClientProvider.CreateDocumentClientAsync(httpRequestMessage);
        ArgumentNullException.ThrowIfNull(documentClient);
        
        // add IDocumentQueryClient into the accessor
        // components need to be able to resolve the same documentQueryClient within the same scope
        IDocumentQueryClientAccessor accessor = _provider.GetRequiredService<IDocumentQueryClientAccessor>();
        accessor.DocumentQueryClient = documentClient;
        return _provider.GetRequiredService<TPage>();
    }
}

