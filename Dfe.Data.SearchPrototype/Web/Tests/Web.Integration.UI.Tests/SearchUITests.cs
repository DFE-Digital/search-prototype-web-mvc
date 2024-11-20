using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Options;
using Dfe.Testing.Pages.DocumentQueryClient.Pages;
using Xunit;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests;

public sealed class SearchUITests : BaseUITest
{
    public SearchUITests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        
    }

    [Fact]
    public async Task SearchPageLoads()
    {
        UIApplicationOptions options = GetTestService<UIApplicationOptions>();

        HttpRequestMessage request = new()
        {
            RequestUri = new Uri(options.BaseUrl, "/")
        };

        var homePage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(request);
    }
}
