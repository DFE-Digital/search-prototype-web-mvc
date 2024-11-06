using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using DfE.Data.SearchPrototype.Web.Tests.Shared;
using FluentAssertions;
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
        HttpRequestMessage request = HttpRequestBuilder.Create()
            .SetPath(Routes.HOME)
            .Build();

        // Act
        IDomQueryClient client = await ResolveService<IDomQueryClientFactory>().CreateClientFromHttpRequestAsync(request);
        
        // Assert
        HomePage homePage = new(client);
        homePage.GetHeading().Should().Be("Search prototype");
    }

    [Fact]
    public async Task Header_Link_IsDisplayed()
    {
        // Arrange
        HttpRequestMessage request = HttpRequestBuilder.Create()
            .SetPath(Routes.HOME)
            .Build();

        // Act
        IDomQueryClient client = await ResolveService<IDomQueryClientFactory>().CreateClientFromHttpRequestAsync(request);
        
        // Assert
        HomePage homePage = new(client);
        homePage.GetNavigationBarHomeText().Should().Be("Home");
    }


    [Fact]
    public async Task Search_Establishment_IsDisplayed()
    {
        // Arrange
        HttpRequestMessage request = HttpRequestBuilder.Create()
            .SetPath(Routes.HOME)
            .Build();

        // Assert
        IDomQueryClient client = await ResolveService<IDomQueryClientFactory>().CreateClientFromHttpRequestAsync(request);

        // Assert
        // TODO expand to form parts need to be able to query within form container at the page level.
        HomePage homePage = new(client);
        homePage.GetSearchHeading().Should().Be("Search");
        homePage.GetSearchSubheading().Should().Be("Search establishments");
        homePage.IsSearchFormExists().Should().BeTrue();
        homePage.IsSearchInputExists().Should().BeTrue();
        homePage.GetSearchFormInputName().Should().Be(Routes.SEARCH_KEYWORD_QUERY);
        homePage.IsSearchButtonExists().Should().BeTrue();
    }

    [Fact]
    public async Task Search_ByName_Returns_LessThan100_Results()
    {
        // TODO stub out search results?
        // Arrange
        HttpRequestMessage searchByKeywordRequest = HttpRequestBuilder.Create()
            .AddQuery(
                new(
                    key: Routes.SEARCH_KEYWORD_QUERY,
                    value: "One"))
            .Build();

        // Act
        IDomQueryClient searchResponseClient = await ResolveService<IDomQueryClientFactory>().CreateClientFromHttpRequestAsync(searchByKeywordRequest);
        
        // Assert
        HomePage searchResultsPage = new(searchResponseClient);
        searchResultsPage.GetSearchResultsText().Should().Contain("Result");
        searchResultsPage.GetSearchResultsContainerCount().Should().BeLessThan(100);
    }

    [Theory]
    [InlineData("St")]
    [InlineData("Jos")]
    [InlineData("Cath")]
    [InlineData("Academy")]
    public async Task Search_ByPartialName_ReturnsMultipleResults(string keyword)
    {
        //TODO stub out results?
        // Arrange
        HttpRequestMessage searchByKeywordRequest = HttpRequestBuilder.Create()
            .AddQuery(
                new(
                    key: Routes.SEARCH_KEYWORD_QUERY,
                    value: keyword))
            .Build();

        // Act
        IDomQueryClient searchResponseClient = await ResolveService<IDomQueryClientFactory>().CreateClientFromHttpRequestAsync(searchByKeywordRequest);

        // Assert
        HomePage searchResultsPage = new(searchResponseClient);
        searchResultsPage.GetSearchResultsText().Should().Contain("Result");
        searchResultsPage.GetSearchResultsContainerCount().Should().BeGreaterThan(1);
        searchResultsPage.GetSearchResultsHeadings().Should().AllSatisfy((t => t.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
    }

    [Fact]
    public async Task Search_ByPartialName_Returns_NoResults()
    {
        //TODO stub out results?
        // Arrange
        const string noResultsSearchKeyword = "zzz";
        HttpRequestMessage searchByKeywordRequest = HttpRequestBuilder.Create()
            .AddQuery(
                new(
                    key: Routes.SEARCH_KEYWORD_QUERY,
                    value: noResultsSearchKeyword))
            .Build();

        // Act
        IDomQueryClient searchResponseClient = await ResolveService<IDomQueryClientFactory>().CreateClientFromHttpRequestAsync(searchByKeywordRequest);

        // Assert
        HomePage searchResultsPage = new(searchResponseClient);
        searchResultsPage.GetNoSearchResultsHeading().Should().Be("Sorry no results found please amend your search criteria");
    }

    [Fact]
    public async Task Filter_Controls_Are_Displayed()
    {
        //TODO stub out facets?
        HttpRequestMessage request = HttpRequestBuilder.Create()
            .AddQuery(new(
                key: "searchKeyWord", value: "Academy"))
            .Build();

        IDomQueryClient client = await ResolveService<IDomQueryClientFactory>()
            .CreateClientFromHttpRequestAsync(request);

        HomePage homePage = new(client);
        homePage.GetFiltersHeading().Should().Be("Filters");
        homePage.GetApplyFiltersText().Should().Be("Apply filters");
        homePage.GetClearFiltersText().Should().Be("Clear filters");
        //TODO Add check for GetAttribute type=submit
        //TODO Add check for GetAttribute type=submit
    }

    [Fact]
    public async Task Filter_ByEstablishmentStatus_Checkboxes_And_Labels_Displayed()
    {
        //TODO stub out SearchResults and Facets?
        HttpRequestMessage request = HttpRequestBuilder.Create()
            .AddQuery(new(
                key: "searchKeyWord", value: "Academy"))
            .Build();

        IDomQueryClient client = await ResolveService<IDomQueryClientFactory>()
            .CreateClientFromHttpRequestAsync(request);

        HomePage homePage = new(client);

        IEnumerable<KeyValuePair<string, string>> expectedFilterValuesToLabels = [
            new("Open", "Open (887)"),
            new("Closed", "Closed (746)"),
            new("Open, but proposed to close", "Open, but proposed to close (7)"),
            new("Proposed to open", "Proposed to open (6)")
        ];
        homePage.GetEstablishmentStatusFiltersHeading().Should().Be("Establishment status");
        homePage.GetEstablishmentStatusFiltersByValueToLabel().Should().BeEquivalentTo(expectedFilterValuesToLabels); // () => List<KeyValuePairs<ValueOfCheckbox, LabelOfTheCheckbox>>
    }

    /*
    [Fact]
    public async Task Filters_AreDisplayed()
    {
        IDomQueryClient client = await ResolveTestService<IDomQueryClientFactory>()
            .CreateAsync("/?searchKeyWord=Academy");
        HomePage homePage = new(client);


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
    }*/
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

