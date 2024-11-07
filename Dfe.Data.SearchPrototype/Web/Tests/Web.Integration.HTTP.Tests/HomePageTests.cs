using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Dfe.Data.SearchPrototype.Infrastructure.DataTransferObjects;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using DfE.Data.SearchPrototype.Web.Tests.Shared;
using DfE.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
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
    public async Task Search_Title_IsDisplayed()
    {
        // Arrange
        HttpRequestMessage request = new HttpRequestBuilder()
            .SetPath(Routes.HOME)
            .Build();

        HttpClient httpClient = ResolveService<CustomWebApplicationFactory>().CreateClient();

        // Act
        IDocumentClient domQueryClient = await new AngleSharpDocumentClientFactory(httpClient)
            .CreateDocumentClientAsync(request);

        // Assert
        HomePage homePage = new(domQueryClient);
        homePage.GetHeading().Should().Be("Search prototype");
    }

    [Fact]
    public async Task Header_Link_IsDisplayed()
    {
        // Arrange
        HttpClient httpClient = ResolveService<CustomWebApplicationFactory>().CreateClient();

        HttpRequestMessage request = new HttpRequestBuilder()
            .SetPath(Routes.HOME)
            .Build();

        // Act
        IDocumentClient domQueryClient = await new AngleSharpDocumentClientFactory(httpClient)
            .CreateDocumentClientAsync(request);

        // Assert
        HomePage homePage = new(domQueryClient);
        homePage.GetNavigationBarHomeText().Should().Be("Home");
    }


    [Fact]
    public async Task Search_Establishment_IsDisplayed()
    {
        // Arrange
        HttpClient client = ResolveService<CustomWebApplicationFactory>().CreateClient();

        HttpRequestMessage request = new HttpRequestBuilder()
            .SetPath(Routes.HOME)
            .Build();

        // Act
        IDocumentClient domQueryClient = await new AngleSharpDocumentClientFactory(client).CreateDocumentClientAsync(request);

        // Assert
        // TODO expand to form parts need to be able to query within form container at the page level.
        HomePage homePage = new(domQueryClient);
        homePage.GetSearchHeading().Should().Be("Search");
        homePage.GetSearchSubheading().Should().Be("Search establishments");
        homePage.IsSearchFormExists().Should().BeTrue();
        homePage.IsSearchInputExists().Should().BeTrue();
        homePage.GetSearchFormInputName().Should().Be(Routes.SEARCH_KEYWORD_QUERY);
        homePage.IsSearchButtonExists().Should().BeTrue();
    }

    [Fact]
    public async Task Search_ByPartialName_Returns_NoResults()
    {
        // Arrange

        // Stub the search response
        WebApplicationFactory<Program> serverFactory =
            ResolveService<CustomWebApplicationFactory>()
            .WithWebHostBuilder((builder) =>
            {
                builder.ConfigureServices((services) =>
                {
                    services.RemoveAll<ISearchByKeywordClientProvider>();
                    services.AddSingleton<SearchResponseBuilder>();
                    services.AddSingleton<ISearchByKeywordClientProvider, DummySearchByKeywordClientProviderTestDouble>();
                });
            });

        serverFactory.Services.GetRequiredService<SearchResponseBuilder>().ClearEstablishments();
        HttpClient client = serverFactory.CreateClient();

        // Build the request 
        HttpRequestMessage searchByKeywordRequest =
            new HttpRequestBuilder()
                .AddQueryParameter(
                    new(
                        key: Routes.SEARCH_KEYWORD_QUERY,
                        value: "ANY_NO_RESULT_KEYWORD"))
                .Build();

        // Act
        AngleSharpDocumentClientFactory searchDocumentClientFactory = new(client);
        IDocumentClient searchResponseClient = await searchDocumentClientFactory.CreateDocumentClientAsync(searchByKeywordRequest);

        // Assert
        HomePage searchResultsPage = new(searchResponseClient);
        searchResultsPage.GetNoSearchResultsHeading().Should().Be("Sorry no results found please amend your search criteria");
    }


    [Fact]
    public async Task Search_ByName_Returns_A_Result()
    {
        // Arrange

        // Stub the search response
        WebApplicationFactory<Program> serverFactory =
            ResolveService<CustomWebApplicationFactory>()
            .WithWebHostBuilder((builder) =>
            {
                builder.ConfigureServices((services) =>
                {
                    services.RemoveAll<ISearchByKeywordClientProvider>();
                    services.AddSingleton<SearchResponseBuilder>();
                    services.AddSingleton<ISearchByKeywordClientProvider, DummySearchByKeywordClientProviderTestDouble>();
                });
            });

        serverFactory.Services.GetRequiredService<SearchResponseBuilder>()
            .AddEstablishment(new()
            {
                TYPEOFESTABLISHMENTNAME = "Blah",
                ESTABLISHMENTNAME = "Blah",
                id = "100000",
                PHASEOFEDUCATION = "Blah",
                ESTABLISHMENTSTATUSNAME = "Something"
            });

        HttpClient client = serverFactory.CreateClient();
        
        // Build the request 
        HttpRequestMessage searchByKeywordRequest = ResolveService<HttpRequestBuilder>()
            .AddQueryParameter(
                new(
                    key: Routes.SEARCH_KEYWORD_QUERY,
                    value: "One"))
            .Build();

        // Act
        AngleSharpDocumentClientFactory documentClientFactory = new(client);
        IDocumentClient searchResponseDocumentClient = await documentClientFactory.CreateDocumentClientAsync(searchByKeywordRequest);

        // Assert
        HomePage searchResultsPage = new(searchResponseDocumentClient);
        searchResultsPage.GetSearchResultsText().Should().Contain("Result");
        searchResultsPage.GetSearchResultsContainerCount().Should().Be(1);
    }

    [Theory]
    [InlineData("St")]
    [InlineData("Jos")]
    [InlineData("Cath")]
    [InlineData("Academy")]
    public async Task Search_ByPartialName_ReturnsMultipleResults(string keyword)
    {
        // Arrange

        // Stub the search response
        WebApplicationFactory<Program> serverFactory =
            ResolveService<CustomWebApplicationFactory>()
            .WithWebHostBuilder((builder) =>
            {
                builder.ConfigureServices((services) =>
                {
                    services.RemoveAll<ISearchByKeywordClientProvider>();
                    services.AddSingleton<SearchResponseBuilder>();
                    services.AddSingleton<ISearchByKeywordClientProvider, DummySearchByKeywordClientProviderTestDouble>();
                });
            });

        serverFactory.Services.GetRequiredService<SearchResponseBuilder>()
            .AddEstablishment(new()
            {
                TYPEOFESTABLISHMENTNAME = "Blah",
                ESTABLISHMENTNAME = "Blah",
                id = "100000",
                PHASEOFEDUCATION = "Blah",
                ESTABLISHMENTSTATUSNAME = "Something"
            })
            .AddEstablishment(new()
            {
                TYPEOFESTABLISHMENTNAME = "Blah2",
                ESTABLISHMENTNAME = "Blah2",
                id = "100001",
                PHASEOFEDUCATION = "Blah2",
                ESTABLISHMENTSTATUSNAME = "Something2"
            });

        HttpClient client = serverFactory.CreateClient();

        HttpRequestMessage searchByKeywordRequest = new HttpRequestBuilder()
            .AddQueryParameter(
                new(
                    key: Routes.SEARCH_KEYWORD_QUERY,
                    value: keyword))
            .Build();


        // Act
        AngleSharpDocumentClientFactory documentClientFactory = new(client);
        IDocumentClient searchResponseDocumentClient = await documentClientFactory.CreateDocumentClientAsync(searchByKeywordRequest);

        // Assert
        HomePage searchResultsPage = new(searchResponseDocumentClient);
        searchResultsPage.GetSearchResultsText().Should().Contain("Result");
        searchResultsPage.GetSearchResultsContainerCount().Should().Be(2);
        // TODO ASSERT THE ACTUAL VALUES BEING RETURNED
        searchResultsPage.GetSearchResultsHeadings().Should().AllSatisfy((t => t.Should().ContainEquivalentOf(keyword)));
    }

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
    }
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

}