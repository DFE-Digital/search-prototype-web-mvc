using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
using Dfe.Testing.Pages;
using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver;
using Dfe.Testing.Pages.Pages;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests;

public sealed class ExampleUITest : BaseEndToEndTest
{
    public ExampleUITest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    // TODO option->navigateToUri = true to dictate to the provider if it should create a document or reuse an existing document
    // internal logic across providers to have a CachedDocumentFactory?

    [Fact]
    public async Task TestConsumptionOf_FullQUeryClient_AsyncScope()
    {
        HttpRequestMessage request = new()
        {
            RequestUri = new("https://searchprototype.azurewebsites.net/")
        };

        var homePage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(request);
        homePage.Search.GetSubheading().Should().Be("Search establishments");
    }
}
