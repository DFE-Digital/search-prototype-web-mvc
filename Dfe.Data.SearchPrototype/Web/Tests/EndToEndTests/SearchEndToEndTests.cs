using Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests.Options;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.ValueObject;
using Dfe.Testing.Pages.Pages;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests;

public sealed class SearchEndToEndTests : BaseEndToEndTest
{
    public SearchEndToEndTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    // TODO option->navigateToUri = true to dictate to the provider if it should create a document or reuse an existing document
    // internal logic across providers to have a CachedDocumentFactory?

    [Fact]
    public async Task Search_Submission_Displays_Search_Results()
    {
        ApplicationOptions options = GetTestService<ApplicationOptions>();
        var homePage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(new()
        {
            RequestUri = new Uri(options.BaseUrl, "/")
        });

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

