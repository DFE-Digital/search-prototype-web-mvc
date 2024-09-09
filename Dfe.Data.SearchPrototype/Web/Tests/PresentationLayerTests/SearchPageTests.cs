using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.Web.Tests.PageObjectModel;
using Dfe.Data.SearchPrototype.Web.Tests.Shared;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.PresentationLayerTests;

public class SearchPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string uri = "http://localhost:5000";
    private readonly WebApplicationFactory<Program> _factory;

    public SearchPageTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Search_ByName_WithMultipleResults()
    {
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        var client = HostWithMockUseCaseWithResponse(useCaseResponse)
            .CreateClient();

        var response = await client.GetAsync(uri);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        var formElement = document.QuerySelector<IHtmlFormElement>(SearchPage.SearchForm.Criteria);
        var formButton = document.QuerySelector<IHtmlButtonElement>(SearchPage.SearchButton.Criteria);

        var formResponse = await client.SendAsync(
            formElement!,
            formButton!,
            new Dictionary<string, string>
            {
                ["searchKeyWord"] = "anything - I've mocked the response from the use-case regardless of the request"
            });

        var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

        resultsPage.QuerySelector(SearchPage.SearchResultsNumber.Criteria)!
            .TextContent.Should().Contain("Results");
        resultsPage.GetMultipleElements(SearchPage.SearchResultLinks.Criteria)
            .Count().Should().Be(useCaseResponse.EstablishmentResults.Establishments.Count);
    }

    private WebApplicationFactory<Program> HostWithMockUseCaseWithResponse(SearchByKeywordResponse response)
    {
        var useCase = new SearchByKeywordUseCaseMockBuilder()
            .WithHandleRequestReturnValue(response)
            .Create();

        return _factory.WithWebHostBuilder(
            (IWebHostBuilder builder) =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>>();
                    services.AddScoped<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>>(provider => useCase);
                });
            }
        );
    }
}