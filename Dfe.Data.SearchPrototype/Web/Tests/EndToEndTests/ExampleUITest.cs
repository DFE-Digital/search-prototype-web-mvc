using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.ValueObject;
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
    public async Task Search_Submission_Displays_Search_Results()
    {
        HttpRequestMessage request = new()
        {
            RequestUri = new("https://localhost:7042/")
        };

        var homePage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(request);
        homePage.Search.SearchForEstablishmentWith("Col");
        homePage.Search.SubmitSearch();
        IEnumerable<EstablishmentSearchResult> searchResults = homePage.Search.SearchResults.GetResults();
        searchResults.Should().NotBeNullOrEmpty();
        searchResults.ToList().ForEach((result) =>
        {
            result.Name.Should().NotBeNullOrEmpty();
            result.Urn.Should().NotBeNullOrEmpty();
            result.TypeOfEstablishment.Should().NotBeNullOrEmpty();
            result.Status.Should().NotBeNullOrEmpty();
            result.Phase.Should().NotBeNullOrEmpty();
        });
    }
}
