using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.ValueObject;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles.Builder;
using Dfe.Testing.Pages.DocumentQueryClient.Pages;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.AnchorLink;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.Inputs;
using Dfe.Testing.Pages.Http;
using Dfe.Testing.Pages.WebApplicationFactory;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests
{
    public class HomePageTests : BaseHttpTest
    {
        private const string HOME_ROUTE = "/";
        private readonly string SEARCH_KEYWORD_QUERY = "searchKeyWord";

        public HomePageTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task ServiceName_Link_IsDisplayed()
        {
            // Arrange

            HttpRequestMessage httpRequest = GetTestService<IHttpRequestBuilder>()
                .SetPath(HOME_ROUTE)
                .Build();

            // Act
            HomePage homePage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(httpRequest);

            // Assert   
            AnchorLink expectedHeadingLink = new()
            {
                LinkValue = HOME_ROUTE,
                Text = "Search prototype",
                OpensInNewTab = false
            };

            homePage.NavigationBar.GetHeading().Should().Be(expectedHeadingLink);
        }

        [Fact]
        public async Task Header_Link_IsDisplayed()
        {
            // Arrange
            HttpRequestMessage httpRequest = GetTestService<IHttpRequestBuilder>()
                .SetPath(HOME_ROUTE)
                .Build();

            // Act
            HomePage homePage = await GetTestService<IPageFactory>()
                .CreatePageAsync<HomePage>(httpRequest);

            // Assert
            AnchorLink headingLink = new()
            {
                LinkValue = HOME_ROUTE,
                Text = "Home",
                OpensInNewTab = false
            };

        homePage.NavigationBar.GetHome().Should().Be(headingLink);
    }

    [Fact]
    public async Task SearchEstablishmentForm_IsDisplayed()
    {
        // Arrange
        HttpRequestMessage homePageRequest = GetTestService<IHttpRequestBuilder>()
            .SetPath(HOME_ROUTE)
            .Build();

        // Act
        HomePage homePage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(homePageRequest);

        // Assert

        // TODO expand to form parts need to be able to query within form container at the page level.

        Input textInput = new()
        {
            Name = "searchKeyWord",
            Value = "",
            PlaceHolder = "Search by keyword",
            Type = "text"
        };

        homePage.Search.GetHeading().Should().Be("Search");
        homePage.Search.GetSubheading().Should().Be("Search establishments");
        homePage.Search.GetSearchInput().Should().Be(textInput);
    }

    [Fact]
    public async Task Search_ByPartialName_Returns_NoResults()
    {
        // Arrange
        MockSearchResponseWith(
            searchResponseBuilder => searchResponseBuilder.ClearEstablishments());

        HttpRequestMessage searchByKeywordRequest = GetTestService<IHttpRequestBuilder>()
            .AddQueryParameter(
                new(
                    key: SEARCH_KEYWORD_QUERY,
                    value: "ANY_NO_RESULT_KEYWORD"))
            .Build();

        // Act
        HomePage searchResultsPage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(searchByKeywordRequest);

        // Assert
        searchResultsPage.Search.GetNoSearchResultsMessage().Trim().Should().Be("Sorry no results found please amend your search criteria");
    }

    [Fact]
    public async Task Search_ByName_Returns_LessThan100_Results()
    {
        // Arrange
        List<EstablishmentSearchResult> expectedSearchResults =
        [
            new EstablishmentSearchResult(
                    Name: "TestEstablishmentName",
                    Urn: "100000",
                    TypeOfEstablishment: "TestTypeOfEstab",
                    Phase: "TestEstablishmentPhase",
                    Status: "TestEstablishmentStatus")
        ];

        MockSearchResponseWithEstablishments(expectedSearchResults);

        HttpRequestMessage searchByKeywordRequest = GetTestService<IHttpRequestBuilder>()
         .AddQueryParameter(
             new(
                 key: SEARCH_KEYWORD_QUERY,
                 value: "RETURNS_ONE_RESULT"))
         .Build();

        // Act
        HomePage homePage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(searchByKeywordRequest);

        // Assert
        homePage.Search.SearchResults.GetResultsHeading().Should().Be("1 Result");
        homePage.Search.SearchResults.GetResults().Should().BeEquivalentTo(expectedSearchResults);

        //TODO labels on the search results
    }

    [Fact]
    public async Task Search_ByName_ReturnsMultipleResults()
    {
        List<EstablishmentSearchResult> expectedSearchResults =
        [
            new(
                    Name: "TestName1",
                    Urn: "100000",
                    TypeOfEstablishment: "MyTypeOfEstab",
                    Phase: "Blah",
                    Status: "MyStatus"),
                new(
                    Name: "TestName2",
                    Urn: "100001",
                    TypeOfEstablishment: "TypeOfEstab2",
                    Phase: "Blah2",
                    Status: "MyStatus2")
        ];

        MockSearchResponseWithEstablishments(expectedSearchResults);

        HttpRequestMessage searchByKeywordRequest = GetTestService<IHttpRequestBuilder>()
            .AddQueryParameter(
                new(
                    key: SEARCH_KEYWORD_QUERY,
                    value: "RETURNS_MULTIPLE_RESULTS"))
            .Build();

        // Act
        HomePage searchResultsPage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(searchByKeywordRequest);

        // Assert
        searchResultsPage.Search.SearchResults.GetResultsHeading().Should().Be("2 Results");
        searchResultsPage.Search.SearchResults.GetResults().Should().BeEquivalentTo(expectedSearchResults);
    }

    [Fact]
    public async Task Filters_AreDisplayed()
    {
        var response = await GetTestService<HttpClient>().GetAsync("/?searchKeyWord=Academy");
        var document = await response.GetDocumentAsync();

        var applyFiltersButton = document.GetElementText(HomePage.ApplyFiltersButton.Criteria);
        applyFiltersButton.Should().Be("Apply filters");

        var phaseOfEducation = document.GetElementText(HomePage.PhaseOfEducationHeading.Criteria);
        phaseOfEducation.Should().Be("Phase of education");

        var primaryInput = document.GetMultipleElements(HomePage.PrimaryFilterInput.Criteria);
        primaryInput.Should().HaveCount(1);

        var primaryLabel = document.GetElementText(HomePage.PrimaryFilterLabel.Criteria);
        primaryLabel.Should().StartWith("Primary");

        var secondaryInput = document.GetMultipleElements(HomePage.SecondaryFilterInput.Criteria);
        secondaryInput.Should().HaveCount(1);

        var secondaryLabel = document.GetElementText(HomePage.SecondaryFilterLabel.Criteria);
        secondaryLabel.Should().StartWith("Secondary");

        var naInput = document.GetMultipleElements(HomePage.NAFilterInput.Criteria);
        naInput.Should().HaveCount(1);

        var naLabel = document.GetElementText(HomePage.NAFilterLabel.Criteria);
        naLabel.Should().StartWith("Not applicable");

        var allThroughInput = document.GetMultipleElements(HomePage.AllThroughFilterInput.Criteria);
        allThroughInput.Should().HaveCount(1);

        var allThroughLabel = document.GetElementText(HomePage.AllThroughFilterLabel.Criteria);
        allThroughLabel.Should().StartWith("All-through");

        var middleDeemedSecondaryInput = document.GetMultipleElements(HomePage.MiddleDeemedSecondaryFilterInput.Criteria);
        middleDeemedSecondaryInput.Should().HaveCount(1);

        var middleDeemedSecondaryLabel = document.GetElementText(HomePage.MiddleDeemedSecondaryFilterLabel.Criteria);
        middleDeemedSecondaryLabel.Should().StartWith("Middle deemed secondary");

        var sixteenPlusInput = document.GetMultipleElements(HomePage.SixteenPlusFilterInput.Criteria);
        sixteenPlusInput.Should().HaveCount(1);

        var sixteenPlusLabel = document.GetElementText(HomePage.SixteenPlusFilterLabel.Criteria);
        sixteenPlusLabel.Should().StartWith("16 plus");

        var middleDeemedPrimaryInput = document.GetMultipleElements(HomePage.MiddleDeemedPrimaryFilterInput.Criteria);
        middleDeemedPrimaryInput.Should().HaveCount(1);

        var middleDeemedPrimaryLabel = document.GetElementText(HomePage.MiddleDeemedPrimaryFilterLabel.Criteria);
        middleDeemedPrimaryLabel.Should().StartWith("Middle deemed primary");

        var establishmentStatusName = document.GetElementText(HomePage.EstablishmentStatusNameHeading.Criteria);
        establishmentStatusName.Should().Be("Establishment status");

        var openInput = document.GetMultipleElements(HomePage.OpenFilterInput.Criteria);
        openInput.Should().HaveCount(1);

        var openLabel = document.GetElementText(HomePage.OpenFilterLabel.Criteria);
        openLabel.Should().StartWith("Open");

        var closedInput = document.GetMultipleElements(HomePage.ClosedFilterInput.Criteria);
        closedInput.Should().HaveCount(1);

        var closedLabel = document.GetElementText(HomePage.ClosedFilterLabel.Criteria);
        closedLabel.Should().StartWith("Closed");

        var proposedToOpenInput = document.GetMultipleElements(HomePage.ProposedToOpenFilterInput.Criteria);
        proposedToOpenInput.Should().HaveCount(1);

        var proposedToOpenLabel = document.GetElementText(HomePage.ProposedToOpenFilterLabel.Criteria);
        proposedToOpenLabel.Should().StartWith("Proposed to open");

        var openProposedToCloseInput = document.GetMultipleElements(HomePage.OpenProposedToCloseFilterInput.Criteria);
        openProposedToCloseInput.Should().HaveCount(1);

        var openProposedToCloseLabel = document.GetElementText(HomePage.OpenProposedToCloseFilterLabel.Criteria);
        openProposedToCloseLabel.Should().StartWith("Open, but proposed to close");
    }

    private void MockSearchResponseWithEstablishments(IEnumerable<EstablishmentSearchResult> results)
     =>
        results.ToList().ForEach((establishment)
            => MockSearchResponseWith((searchResponseBuilder)
                => searchResponseBuilder.AddEstablishment(
                    (establishmentBuilder) =>
                        establishmentBuilder
                            .SetTypeOfEstablishment(establishment.TypeOfEstablishment)
                            .SetName(establishment.Name)
                            .SetId(establishment.Urn)
                            .SetPhaseOfEducation(establishment.Phase)
                            .SetStatus(establishment.Status))));

    private void MockSearchResponseWith(Action<SearchResponseBuilder> configureSearchResponse)
    {
        GetTestService<IConfigureWebHostHandler>()
            .ConfigureWith((builder) =>
            {
                builder.ConfigureServices((services) =>
                {
                    services.RemoveAll<ISearchByKeywordClientProvider>();
                    services.AddSingleton<ISearchByKeywordClientProvider, SearchByKeywordClientProviderTestDouble>()
                        .AddSingleton<IEstablishmentBuilder, EstablishmentBuilder>()
                        .AddSingleton<SearchResponseBuilder>();
                });
            });

        SearchResponseBuilder builder = GetTestService<WebApplicationFactory<Program>>().Services
            .GetRequiredService<SearchResponseBuilder>();

        configureSearchResponse(builder);
    }
}
}
