using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Testing.Pages.Pages;
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
        HttpRequestMessage request = new()
        {
            RequestUri = new("https://searchprototype.azurewebsites.net/")
        };

        var homePage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(request);
    }
}
