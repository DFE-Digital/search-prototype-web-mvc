using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.ValueObjects;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Dfe.Testing.Pages.Http;
using Dfe.Testing.Pages.Pages.Components.AnchorLink;
using Dfe.Testing.Pages.Pages.Components.Input;
using Dfe.Testing.Pages.Pages.Factory;
using Dfe.Testing.Pages.WebApplicationFactory;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using Xunit.Abstractions;
using static Dfe.Data.SearchPrototype.Web.Tests.Shared.Constants;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests;

public class HomePageTests : BaseHttpTest
{
    public HomePageTests(ITestOutputHelper logger) : base(logger)
    {
    }

    [Fact]
    public async Task ServiceName_Link_IsDisplayed()
    {
        // Arrange
        HttpRequestMessage httpRequest = GetTestService<IHttpRequestBuilder>()
            .SetPath(Routes.HOME)
            .Build();

        // Act
        HomePage homePage = await GetTestService<IPageFactory>()
            .CreatePageAsync<HomePage>(httpRequest);

        // Assert
        Link expectedHeadingLink = new(
            link: Routes.HOME,
            text: "Search prototype",
            opensInNewTab: false);

        homePage.NavigationBar.GetHeading().Should().Be(expectedHeadingLink);
    }

    //TODO GOV.UK link

    [Fact]
    public async Task Header_Link_IsDisplayed()
    {
        // Arrange
        HttpRequestMessage httpRequest = GetTestService<IHttpRequestBuilder>()
            .SetPath(Routes.HOME)
            .Build();

        // Act
        HomePage homePage = await GetTestService<IPageFactory>()
            .CreatePageAsync<HomePage>(httpRequest);

        // Assert
        Link headingLink = new(
            link: Routes.HOME,
            text: "Home",
            opensInNewTab: false);

        homePage.NavigationBar.GetHome().Should().Be(headingLink);
    }

    [Fact]
    public async Task SearchEstablishmentForm_IsDisplayed()
    {
        // Arrange
        HttpRequestMessage homePageRequest = GetTestService<IHttpRequestBuilder>()
            .SetPath(Routes.HOME)
            .Build();

        // Act
        HomePage homePage = await GetTestService<IPageFactory>()
            .CreatePageAsync<HomePage>(homePageRequest);

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

        //homePage.Search.Form
        /*        homePage.IsSearchFormExists().Should().BeTrue();
                homePage.IsSearchInputExists().Should().BeTrue();
                homePage.GetSearchFormInputName().Should().Be(Routes.SEARCH_KEYWORD_QUERY);
                homePage.IsSearchButtonExists().Should().BeTrue();*/
    }

    //TODO add one in for no results (client validation, server validation)?

    [Fact]
    public async Task Search_ByPartialName_Returns_NoResults()
    {
        // Arrange
        MockSearchResponseWith(
            searchResponseBuilder => searchResponseBuilder.ClearEstablishments());

        HttpRequestMessage searchByKeywordRequest = GetTestService<IHttpRequestBuilder>()
            .AddQueryParameter(
                new(
                    key: Routes.SEARCH_KEYWORD_QUERY,
                    value: "ANY_NO_RESULT_KEYWORD"))
            .Build();

        // Act
        HomePage searchResultsPage = await GetTestService<IPageFactory>()
            .CreatePageAsync<HomePage>(searchByKeywordRequest);

        // Assert
        searchResultsPage.Search.GetNoSearchResultsMessage().Should().Be("Sorry no results found please amend your search criteria");
    }

    [Fact]
    public async Task Search_ByName_Returns_A_Result()
    {
        // Arrange
        List<EstablishmentSearchResult> establishmentSearchResults =
        [
            new EstablishmentSearchResult(
                name: "Blah2",
                urn: "100000",
                typeOfEstablishment: "MyTypeOfEstab",
                phase: "Blah3",
                status: "MyStatus")
        ];

        establishmentSearchResults.ForEach((searchResult) =>
            MockSearchResponseWith(
                (searchResponseBuilder) => searchResponseBuilder.AddEstablishment(
                    (establishmentBuilder) =>
                    {
                        establishmentBuilder
                            .SetTypeOfEstablishment(searchResult.typeOfEstablishment)
                            .SetName(searchResult.name)
                            .SetId(searchResult.urn)
                            .SetPhaseOfEducation(searchResult.phase)
                            .SetStatus(searchResult.status);
                    })));

        /*.SetAddress("123 Cherry lane")*/


        HttpRequestMessage searchByKeywordRequest = GetTestService<IHttpRequestBuilder>()
            .AddQueryParameter(
                new(
                    key: Routes.SEARCH_KEYWORD_QUERY,
                    value: "One"))
            .Build();

        // Act
        HomePage homePage = await GetTestService<IPageFactory>()
            .CreatePageAsync<HomePage>(searchByKeywordRequest);

        // Assert
        homePage.Search.SearchResults.GetResultsHeading().Should().Be("1 Result");
        homePage.Search.SearchResults.GetResults().Should().BeEquivalentTo(establishmentSearchResults);

        //TODO labels on the search results
    }

    [Fact]
    public async Task Search_ByPartialName_ReturnsMultipleResults()
    {
        List<EstablishmentSearchResult> expectedSearchResults =
        [
            new(
                name: "TestName1",
                urn: "100000",
                typeOfEstablishment: "MyTypeOfEstab",
                phase: "Blah",
                status: "MyStatus"),

            new(
                name: "TestName2",
                urn: "100001",
                typeOfEstablishment: "TypeOfEstab2",
                phase: "Blah2",
                status: "MyStatus2")
        ];

        expectedSearchResults.ForEach((establishment)
            => MockSearchResponseWith((searchResponseBuilder)
                => searchResponseBuilder.AddEstablishment(
                    (establishmentBuilder) =>
                        establishmentBuilder
                            .SetTypeOfEstablishment(establishment.typeOfEstablishment)
                            .SetName(establishment.name)
                            .SetId(establishment.urn)
                            .SetPhaseOfEducation(establishment.phase)
                            .SetStatus(establishment.status))));

        HttpRequestMessage searchByKeywordRequest = GetTestService<IHttpRequestBuilder>()
            .AddQueryParameter(
                new(
                    key: Routes.SEARCH_KEYWORD_QUERY,
                    value: "RETURNS_MULTIPLE_RESULTS"))
            .Build();

        // Act
        HomePage searchResultsPage = await GetTestService<IPageFactory>()
            .CreatePageAsync<HomePage>(searchByKeywordRequest);

        // Assert
        searchResultsPage.Search.SearchResults.GetResultsHeading().Should().Be("2 Results");
        searchResultsPage.Search.SearchResults.GetResults().Should().BeEquivalentTo(expectedSearchResults);
    }

    /*
    [Fact]
    public async Task Filter_Controls_Are_Displayed()
    {
        //TODO stub out facets?

        // Arrange
        HttpClient client = ResolveService<CustomWebApplicationFactory>().CreateClient();

        HttpRequestMessage searchByKeywordRequest = new HttpRequestBuilder()
            .AddQueryParameter(new(
                key: "searchKeyWord", value: "Academy"))
            .Build();

        // Act
        IDocumentClient searchResponseDocumentClient = await new AngleSharpDocumentClientFactory(client).CreateDocumentClientAsync(searchByKeywordRequest);

        HomePage homePage = new(searchResponseDocumentClient);
        homePage.GetFiltersHeading().Should().Be("Filters");
        homePage.GetApplyFiltersText().Should().Be("Apply filters");
        homePage.GetClearFiltersText().Should().Be("Clear filters");
        //TODO Add check for GetAttribute type=submit
        //TODO Add check for GetAttribute type=submit
    }

    [Fact]
    public async Task FilterS_ByEstablishmentStatus_Checkboxes_And_Labels_Displayed()
    {
        //TODO stub out facets - test no facets configured, 1 facet, many facets

        // Arrange
        HttpClient client = ResolveService<CustomWebApplicationFactory>().CreateClient();

        HttpRequestMessage request = new HttpRequestBuilder()
            .AddQueryParameter(new(
                key: "searchKeyWord", value: "Academy"))
            .Build();

        // Act
        IDocumentClient searchResponseClient = await new AngleSharpDocumentClientFactory(client).CreateDocumentClientAsync(request);

        HomePage homePage = new(searchResponseClient);

        IEnumerable<KeyValuePair<string, string>> expectedFilterValuesToLabels = [
            new("Open", "Open (887)"),
            new("Closed", "Closed (746)"),
            new("Open, but proposed to close", "Open, but proposed to close (7)"),
            new("Proposed to open", "Proposed to open (6)")
        ];
        homePage.GetEstablishmentStatusFiltersHeading().Should().Be("Establishment status");
        homePage.GetEstablishmentStatusFiltersByValueToLabel().Should().BeEquivalentTo(expectedFilterValuesToLabels);
    }

    [Fact]
    public async Task Filters_ByPhaseOfEducation_Checkboxes_And_Labels_Displayed()
    {
        //TODO stub out facets - test no facets configured, 1 facet, many facets

        // Arrange
        HttpClient client = ResolveService<CustomWebApplicationFactory>().CreateClient();

        HttpRequestMessage request = new HttpRequestBuilder()
            .AddQueryParameter(new(
                key: "searchKeyWord", value: "Academy"))
            .Build();

        // Act
        IDocumentClient searchResponseClient = await new AngleSharpDocumentClientFactory(client).CreateDocumentClientAsync(request);

        // Assert
        HomePage homePage = new(searchResponseClient);
        IEnumerable<KeyValuePair<string, string>> expectedFilterValuesToLabels = [
            new("Primary", "Primary (951)"),
            new("Not applicable", "Not applicable (457)"),
            new("Secondary, but proposed to close", "Secondary (183)"),
            new("Nursery", "Nursery (26)"),
            new("16 plus", "16 plus (17)"),
            new("All-through", "All-through (9)"),
            new("Middle deemed secondary", "Middle deemed secondary (2)"),
            new("Middle deemed primary", "Middle deemed primary (1)")
        ];

        homePage.GetPhaseOfEducationFiltersHeading().Should().Be("Phase of education");
        homePage.GetPhaseOfEducationFiltersByValueToLabel().Should().BeEquivalentTo(expectedFilterValuesToLabels);
    }*/
    /*
        [Theory]
        [MemberData(nameof(EstablishmentStatusElements))]
        public async Task Apply_EstablishmentStatus_Filter_Submissions(string keyword, string queryParam, string queryParamValue, string element, string establishmentStatus)
        {
            var response = await _client.GetAsync(uri + $"/?searchKeyWord={keyword}&selectedFacets%5B{queryParam}%5D={queryParamValue}");
            var document = await response.GetDocumentAsync();

            var openCheckBox = document.QuerySelector(element)!.GetAttribute("checked");
            openCheckBox.Should().Be("checked");

            var establishmentStatusText = document.QuerySelector(HomePage.SearchResultEstablishmentStatus.Criteria);
            foreach (var text in establishmentStatusText!.Text())
            {
                establishmentStatusText!.TextContent.Should().ContainAny(establishmentStatus);
            }
        }

        [Theory]
        [MemberData(nameof(PhaseOfEducationElements))]
        public async Task Apply_PhaseOfEducation_Filter_Submissions(string keyword, string queryParam, string queryParamValue, string element, string educationPhase)
        {
            var response = await _client.GetAsync(uri + $"/?searchKeyWord={keyword}&selectedFacets%5B{queryParam}%5D={queryParamValue}");
            var document = await response.GetDocumentAsync();

            var openCheckBox = document.QuerySelector(element)!.GetAttribute("checked");
            openCheckBox.Should().Be("checked");

            var educationPhaseText = document.QuerySelector(HomePage.SearchResultEstablishmentPhase.Criteria);
            foreach (var text in educationPhaseText!.Text())
            {
                educationPhaseText!.TextContent.Should().ContainAny(educationPhase);
            }
        }

        [Fact]
        public async Task Clear_Filters_Button_Text_And_Type()
        {
            var response = await _client.GetAsync(uri + "/?searchKeyWord=school&selectedFacets%5BESTABLISHMENTSTATUSNAME%5D=Open");
            var document = await response.GetDocumentAsync();

            var clearFiltersButtonText = document.GetElementText(HomePage.ClearFiltersButton.Criteria);
            clearFiltersButtonText.Should().Be("Clear filters");

            var clearFiltersButtonType = document.QuerySelector(HomePage.ClearFiltersButton.Criteria)!.GetAttribute("type");
            clearFiltersButtonType.Should().Be("submit");
        }

        [Fact]
        public async Task Clear_Filters_Button_Clears_Single_EstablishmentStatus_Filter()
        {
            var response = await _client.GetAsync(uri + "/?searchKeyWord=school&ClearFilters=true&selectedFacets%5BESTABLISHMENTSTATUSNAME%5D=Open");
            var document = await response.GetDocumentAsync();

            var applyFiltersButton = document.QuerySelector(HomePage.ApplyFiltersButton.Criteria);
            applyFiltersButton.Should().NotBeNull();

            var clearFiltersButtonText = document.QuerySelector(HomePage.ClearFiltersButton.Criteria);
            clearFiltersButtonText.Should().NotBeNull();

            var openFilterInput = document.QuerySelector(HomePage.OpenFilterInput.Criteria);
            openFilterInput.Should().NotBeNull();
            openFilterInput!.GetAttribute("checked").Should().Be(null);
        }

        [Fact]
        public async Task Clear_Filters_Button_Clears_Single_PhaseOfEducation_Filter()
        {
            var response = await _client.GetAsync(uri + "/?searchKeyWord=school&ClearFilters=true&selectedFacets%5BPHASEOFEDUCATION%5D=Not+applicable");
            var document = await response.GetDocumentAsync();

            var applyFiltersButton = document.QuerySelector(HomePage.ApplyFiltersButton.Criteria);
            applyFiltersButton.Should().NotBeNull();

            var clearFiltersButtonText = document.QuerySelector(HomePage.ClearFiltersButton.Criteria);
            clearFiltersButtonText.Should().NotBeNull();

            var naFilterInput = document.QuerySelector(HomePage.NAFilterInput.Criteria);
            naFilterInput.Should().NotBeNull();
            naFilterInput!.GetAttribute("checked").Should().Be(null);
        }

        [Fact]
        public async Task Clear_Filters_Button_Clears_Multiple_EstablishmentStatus_Filters()
        {
            var response = await _client.GetAsync(uri + "/?searchKeyWord=school&ClearFilters=true&selectedFacets%5BESTABLISHMENTSTATUSNAME%5D=Open&selectedFacets%5BESTABLISHMENTSTATUSNAME%5D=Closed");
            var document = await response.GetDocumentAsync();

            var applyFiltersButton = document.QuerySelector(HomePage.ApplyFiltersButton.Criteria);
            applyFiltersButton.Should().NotBeNull();

            var clearFiltersButtonText = document.QuerySelector(HomePage.ClearFiltersButton.Criteria);
            clearFiltersButtonText.Should().NotBeNull();

            var openFilterInput = document.QuerySelector(HomePage.OpenFilterInput.Criteria);
            openFilterInput.Should().NotBeNull();
            openFilterInput!.GetAttribute("checked").Should().Be(null);

            var closedFilterInput = document.QuerySelector(HomePage.ClosedFilterInput.Criteria);
            closedFilterInput.Should().NotBeNull();
            closedFilterInput!.GetAttribute("checked").Should().Be(null);
        }

        [Fact]
        public async Task Clear_Filters_Button_Clears_Multiple_PhaseOfEducation_Filters()
        {
            var response = await _client.GetAsync(uri + "/?searchKeyWord=school&ClearFilters=true&selectedFacets%5BPHASEOFEDUCATION%5D=Middle+deemed+secondary&selectedFacets%5BPHASEOFEDUCATION%5D=Middle+deemed+primary");
            var document = await response.GetDocumentAsync();

            var applyFiltersButton = document.QuerySelector(HomePage.ApplyFiltersButton.Criteria);
            applyFiltersButton.Should().NotBeNull();

            var clearFiltersButtonText = document.QuerySelector(HomePage.ClearFiltersButton.Criteria);
            clearFiltersButtonText.Should().NotBeNull();

            var openFilterInput = document.QuerySelector(HomePage.MiddleDeemedSecondaryFilterInput.Criteria);
            openFilterInput.Should().NotBeNull();
            openFilterInput!.GetAttribute("checked").Should().Be(null);

            var closedFilterInput = document.QuerySelector(HomePage.MiddleDeemedPrimaryFilterInput.Criteria);
            closedFilterInput.Should().NotBeNull();
            closedFilterInput!.GetAttribute("checked").Should().Be(null);
        }

        [Fact]
        public async Task Clear_Filters_Button_Clears_Multiple_Filters()
        {
            var response = await _client.GetAsync(uri + "/?searchKeyWord=school&ClearFilters=true&selectedFacets%5BESTABLISHMENTSTATUSNAME%5D=Open&selectedFacets%5BESTABLISHMENTSTATUSNAME%5D=Closed&selectedFacets%5BPHASEOFEDUCATION%5D=Primary&selectedFacets%5BPHASEOFEDUCATION%5D=Secondary");
            var document = await response.GetDocumentAsync();

            var applyFiltersButton = document.QuerySelector(HomePage.ApplyFiltersButton.Criteria);
            applyFiltersButton.Should().NotBeNull();

            var clearFiltersButtonText = document.QuerySelector(HomePage.ClearFiltersButton.Criteria);
            clearFiltersButtonText.Should().NotBeNull();

            var openFilterInput = document.QuerySelector(HomePage.OpenFilterInput.Criteria);
            openFilterInput.Should().NotBeNull();
            openFilterInput!.GetAttribute("checked").Should().Be(null);
        }

        [Fact]
        public async Task Clear_Filters_Button_Clears_All_Filters()
        {
            var response = await _client.GetAsync(uri + "/?searchKeyWord=school&ClearFilters=true&selectedFacets%5BESTABLISHMENTSTATUSNAME%5D=Open&selectedFacets%5BESTABLISHMENTSTATUSNAME%5D=Closed&selectedFacets%5BESTABLISHMENTSTATUSNAME%5D=Open%2C+but+proposed+to+close&selectedFacets%5BESTABLISHMENTSTATUSNAME%5D=Proposed+to+open&selectedFacets%5BPHASEOFEDUCATION%5D=Primary&selectedFacets%5BPHASEOFEDUCATION%5D=Not+applicable&selectedFacets%5BPHASEOFEDUCATION%5D=Secondary&selectedFacets%5BPHASEOFEDUCATION%5D=Middle+deemed+secondary&selectedFacets%5BPHASEOFEDUCATION%5D=Nursery&selectedFacets%5BPHASEOFEDUCATION%5D=Middle+deemed+primary&selectedFacets%5BPHASEOFEDUCATION%5D=All-through&selectedFacets%5BPHASEOFEDUCATION%5D=16+plus");
            var document = await response.GetDocumentAsync();

            var applyFiltersButton = document.QuerySelector(HomePage.ApplyFiltersButton.Criteria);
            applyFiltersButton.Should().NotBeNull();

            var clearFiltersButtonText = document.QuerySelector(HomePage.ClearFiltersButton.Criteria);
            clearFiltersButtonText.Should().NotBeNull();

            var openFilterInput = document.QuerySelector(HomePage.OpenFilterInput.Criteria);
            openFilterInput.Should().NotBeNull();
            openFilterInput!.GetAttribute("checked").Should().Be(null);

            var closedFilterInput = document.QuerySelector(HomePage.ClosedFilterInput.Criteria);
            closedFilterInput.Should().NotBeNull();
            closedFilterInput!.GetAttribute("checked").Should().Be(null);

            var openProposedToCloseFilterInput = document.QuerySelector(HomePage.OpenProposedToCloseFilterInput.Criteria);
            openProposedToCloseFilterInput.Should().NotBeNull();
            openProposedToCloseFilterInput!.GetAttribute("checked").Should().Be(null);

            var proposedToOpenFilterInput = document.QuerySelector(HomePage.ProposedToOpenFilterInput.Criteria);
            proposedToOpenFilterInput.Should().NotBeNull();
            proposedToOpenFilterInput!.GetAttribute("checked").Should().Be(null);

            var primaryFilterInput = document.QuerySelector(HomePage.PrimaryFilterInput.Criteria);
            primaryFilterInput.Should().NotBeNull();
            primaryFilterInput!.GetAttribute("checked").Should().Be(null);

            var naFilterInput = document.QuerySelector(HomePage.NAFilterInput.Criteria);
            naFilterInput.Should().NotBeNull();
            naFilterInput!.GetAttribute("checked").Should().Be(null);

            var secondaryFilterInput = document.QuerySelector(HomePage.SecondaryFilterInput.Criteria);
            secondaryFilterInput.Should().NotBeNull();
            secondaryFilterInput!.GetAttribute("checked").Should().Be(null);

            var middleDeemedSecondaryFilterInput = document.QuerySelector(HomePage.MiddleDeemedSecondaryFilterInput.Criteria);
            middleDeemedSecondaryFilterInput.Should().NotBeNull();
            middleDeemedSecondaryFilterInput!.GetAttribute("checked").Should().Be(null);

            var nurseryFilterInput = document.QuerySelector(HomePage.NurseryFilterInput.Criteria);
            nurseryFilterInput.Should().NotBeNull();
            nurseryFilterInput!.GetAttribute("checked").Should().Be(null);

            var middleDeemedPrimaryFilterInput = document.QuerySelector(HomePage.MiddleDeemedPrimaryFilterInput.Criteria);
            middleDeemedPrimaryFilterInput.Should().NotBeNull();
            middleDeemedPrimaryFilterInput!.GetAttribute("checked").Should().Be(null);

            var allThroughFilterInput = document.QuerySelector(HomePage.AllThroughFilterInput.Criteria);
            allThroughFilterInput.Should().NotBeNull();
            allThroughFilterInput!.GetAttribute("checked").Should().Be(null);

            var sixteenPlusFilterInput = document.QuerySelector(HomePage.SixteenPlusFilterInput.Criteria);
            sixteenPlusFilterInput.Should().NotBeNull();
            sixteenPlusFilterInput!.GetAttribute("checked").Should().Be(null);
        }






        /// <summary>
        /// Object contains search term, query parameter, query parameter value, page object and establishment status.
        /// </summary>
        public static IEnumerable<object[]> EstablishmentStatusElements()
        {
            yield return new object[] { "middle", "ESTABLISHMENTSTATUSNAME", "Open", HomePage.OpenFilterInput.Criteria, "Open" };
            yield return new object[] { "academy", "ESTABLISHMENTSTATUSNAME", "Closed", HomePage.ClosedFilterInput.Criteria, "Closed" };
            yield return new object[] { "school", "ESTABLISHMENTSTATUSNAME", "Proposed+to+open", HomePage.ProposedToOpenFilterInput.Criteria, "Proposed to open" };
            yield return new object[] { "isle", "ESTABLISHMENTSTATUSNAME", "Open%2C+but+proposed+to+close", HomePage.OpenProposedToCloseFilterInput.Criteria, "Open" };
        }

        /// <summary>
        /// Object contains search term, query parameter, query parameter value, page object and phase of education.
        /// </summary>
        public static IEnumerable<object[]> PhaseOfEducationElements()
        {
            yield return new object[] { "west", "PHASEOFEDUCATION", "Primary", HomePage.PrimaryFilterInput.Criteria, "Primary" };
            yield return new object[] { "west", "PHASEOFEDUCATION", "Secondary", HomePage.SecondaryFilterInput.Criteria, "Secondary" };
            yield return new object[] { "west", "PHASEOFEDUCATION", "Not+applicable", HomePage.NAFilterInput.Criteria, "Not applicable" };
            yield return new object[] { "west", "PHASEOFEDUCATION", "All-through", HomePage.AllThroughFilterInput.Criteria, "All-through" };
            yield return new object[] { "west", "PHASEOFEDUCATION", "Middle+deemed+secondary", HomePage.MiddleDeemedSecondaryFilterInput.Criteria, "Middle deemed secondary" };
            yield return new object[] { "west", "PHASEOFEDUCATION", "16+plus", HomePage.SixteenPlusFilterInput.Criteria, "16 plus" };
            yield return new object[] { "west", "PHASEOFEDUCATION", "Middle+deemed+primary", HomePage.MiddleDeemedPrimaryFilterInput.Criteria, "Middle deemed primary" };
        }*/


    private void MockSearchResponseWith(Action<SearchResponseBuilder> configureSearchResponse)
    {

        GetTestService<IConfigureWebHostHandler>()
            .ConfigureWith((builder) =>
            {
                builder.ConfigureServices((services) =>
                {
                    services.RemoveAll<ISearchByKeywordClientProvider>()
                    .AddSingleton<ISearchByKeywordClientProvider, SearchByKeywordClientProviderTestDouble>()
                    .AddSingleton<IEstablishmentBuilder, EstablishmentBuilder>()
                    .AddSingleton<SearchResponseBuilder>();
                });
            });

        SearchResponseBuilder builder = GetTestService<WebApplicationFactory<Program>>().Services
            .GetRequiredService<SearchResponseBuilder>();

        configureSearchResponse(builder);
    }
}