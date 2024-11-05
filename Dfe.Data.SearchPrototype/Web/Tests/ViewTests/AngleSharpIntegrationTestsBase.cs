using AngleSharp;
using AngleSharp.Io;
using AngleSharp.Io.Network;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.ViewTests;

public class AngleSharpIntegrationTestsBase : IClassFixture<IntegrationTestingWebApplicationFactory>
{
    private readonly IBrowsingContext _browsingContext;
    protected readonly IntegrationTestingWebApplicationFactory _factory;
    protected readonly ISearchPage _searchPage;

    public AngleSharpIntegrationTestsBase(IntegrationTestingWebApplicationFactory factory)
    {
        _factory = factory;
        var httpClient = factory.CreateClient();
        _browsingContext = CreateBrowsingContext(httpClient);
        _searchPage = new AngleSharpSearchPageModel(_browsingContext);
    }

    private IBrowsingContext CreateBrowsingContext(HttpClient httpClient)
    {
        var config = AngleSharp.Configuration.Default
            .WithRequester(new HttpClientRequester(httpClient))
            .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

        return BrowsingContext.New(config);
    }
}
