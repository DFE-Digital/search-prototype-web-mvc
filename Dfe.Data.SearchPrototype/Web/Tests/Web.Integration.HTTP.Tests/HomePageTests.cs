﻿using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using FluentAssertions;
using System.Text;
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
        HttpRequestMessage request = HttpRequestBuilder.Create()
            .SetPath(Routes.HOME)
            .Build();

        IDomQueryClient client = await ResolveTestService<IDomQueryClientFactory>().CreateClientFromHttpRequestAsync(request);
        HomePage homePage = new(client);
        homePage.GetHeading().Should().Be("Search prototype");
    }

    [Fact]
    public async Task Header_Link_IsDisplayed()
    {
        HttpRequestMessage request = HttpRequestBuilder.Create()
            .SetPath(Routes.HOME)
            .Build();

        IDomQueryClient client = await ResolveTestService<IDomQueryClientFactory>().CreateClientFromHttpRequestAsync(request);
        HomePage homePage = new(client);
        homePage.GetNavigationBarHomeText().Should().Be("Home");
    }


    [Fact]
    public async Task Search_Establishment_IsDisplayed()
    {
        HttpRequestMessage request = HttpRequestBuilder.Create()
            .SetPath(Routes.HOME)
            .Build();

        // TODO expand to form parts need to be able to query within form container at the page level.
        IDomQueryClient client = await ResolveTestService<IDomQueryClientFactory>().CreateClientFromHttpRequestAsync(request);
        HomePage homePage = new(client);
        homePage.GetSearchHeading().Should().Be("Search");
        homePage.GetSearchSubheading().Should().Be("Search establishments");

        homePage.IsSearchFormExists().Should().BeTrue();
        homePage.IsSearchInputExists().Should().BeTrue();
        homePage.GetSearchFormInputName().Should().Be("searchKeyWord");
        homePage.IsSearchButtonExists().Should().BeTrue();
    }

    [Fact]
    public async Task Search_ByName_Returns_LessThan100_Results()
    {
        HttpRequestMessage request = HttpRequestBuilder.Create()
            .SetPath(Routes.HOME)
            .Build();

        // TODO stub out search results?
        // TODO do we NEED to make a request to home first, or, can we rely on a constant for the searchByKeyword query - and only focus on search submission here?
        IDomQueryClientFactory clientFactory = ResolveTestService<IDomQueryClientFactory>();
        IDomQueryClient client = await clientFactory.CreateClientFromHttpRequestAsync(request);
        var searchInputFormName = new HomePage(client)
            .GetSearchFormInputName()
            .Should().NotBeNullOrEmpty().And.Subject;

        // Build form request
        HttpRequestMessage searchByKeywordRequest = HttpRequestBuilder.Create()
            .AddQuery(
                new(
                    key: searchInputFormName,
                    value: "One"))
            .Build();

        IDomQueryClient searchResponseClient = await clientFactory.CreateClientFromHttpRequestAsync(searchByKeywordRequest);
        HomePage searchResultsPage = new(searchResponseClient);
        searchResultsPage.GetSearchResultsText().Should().Contain("Result");
        searchResultsPage.GetSearchResultsCount().Should().BeLessThan(100);
    }

    /*
       [Fact]
       public async Task Search_ByName_ReturnsMultipleResults()
       {
           var response = await _client.GetAsync("");
           var document = await response.GetDocumentAsync();

           var formElement = document.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria) ?? throw new Exception("Unable to find the sign in form");
           var formButton = document.QuerySelector<IHtmlButtonElement>(HomePage.SearchButton.Criteria) ?? throw new Exception("Unable to find the submit button on search form");

           var formResponse = await _client.SendAsync(
               formElement,
               formButton,
               new Dictionary<string, string>
               {
                   ["searchKeyWord"] = "Academy"
               });

           var resultsPage = await formResponse.GetDocumentAsync();

           resultsPage.GetElementText(HomePage.SearchResultsNumber.Criteria).Should().Contain("Results");
           resultsPage.GetMultipleElements(HomePage.SearchResultsHeadings.Criteria).Count().Should().Be(100);
       }

       [Theory]
       [InlineData("St")]
       [InlineData("Jos")]
       [InlineData("Cath")]
       public async Task Search_ByPartialName_ReturnsResults(string term)
       {
           var response = await _client.GetAsync("");
           var document = await response.GetDocumentAsync();

           var formElement = document.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria) ?? throw new Exception("Unable to find the sign in form");
           var formButton = document.QuerySelector<IHtmlButtonElement>(HomePage.SearchButton.Criteria) ?? throw new Exception("Unable to find the submit button on search form");

           var formResponse = await _client.SendAsync(
               formElement,
               formButton,
               new Dictionary<string, string>
               {
                   ["searchKeyWord"] = term
               });

           var resultsPage = await formResponse.GetDocumentAsync();

           var resultsNumber = resultsPage.GetElementText(HomePage.SearchResultsNumber.Criteria);
           resultsNumber.Should().Contain("Results");

           var resultsHeadingsText = resultsPage.GetMultipleElements(HomePage.SearchResultsHeadings.Criteria);
           resultsHeadingsText.Should().HaveCountGreaterThan(1);

           var resultsHeadings = resultsPage.QuerySelector(HomePage.SearchResultsHeadings.Criteria);
           foreach (var headings in resultsHeadings!.Text())
           {
               resultsHeadings!.TextContent.Should().ContainAny(term);
           }

       }

       [Theory]
       [InlineData("abcd")]
       [InlineData("zzz")]
       public async Task Search_ByName_NoMatch_ReturnsNoResults(string searchTerm)
       {
           var response = await _client.GetAsync("");
           var document = await response.GetDocumentAsync();

           var formElement = document.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria) ?? throw new Exception("Unable to find the sign in form");
           var formButton = document.QuerySelector<IHtmlButtonElement>(HomePage.SearchButton.Criteria) ?? throw new Exception("Unable to find the submit button on search form");

           var formResponse = await _client.SendAsync(
               formElement,
               formButton,
               new Dictionary<string, string>
               {
                   ["searchKeyWord"] = searchTerm
               });

           var resultsPage = await formResponse.GetDocumentAsync();

           var noResultText = resultsPage.GetElementText(HomePage.SearchNoResultText.Criteria);
           noResultText.Should().Contain("Sorry no results found please amend your search criteria");
       }
    */

    [Fact]
    public async Task Filter_Controls_Are_Displayed()
    {
        HttpRequestMessage request = HttpRequestBuilder.Create()
            .AddQuery(new(
                key: "searchKeyWord", value: "Academy"))
            .Build();

        IDomQueryClient client = await ResolveTestService<IDomQueryClientFactory>()
            .CreateClientFromHttpRequestAsync(request);
        
        HomePage homePage = new(client);
        homePage.GetFiltersHeading().Should().Be("Filters");
        homePage.GetApplyFiltersText().Should().Be("Apply filters");
        homePage.GetClearFiltersText().Should().Be("Clear filters");
    }

    [Fact]
    public async Task Filter_ByEstablishmentStatus_Checkboxes_And_Labels_Displayed()
    {
        HttpRequestMessage request = HttpRequestBuilder.Create()
            .AddQuery(new(
                key: "searchKeyWord", value: "Academy"))
            .Build();
        IDomQueryClient client = await ResolveTestService<IDomQueryClientFactory>()
            .CreateClientFromHttpRequestAsync(request);

        HomePage homePage = new(client);
        homePage.GetEstablishmentStatusFiltersHeading().Should().Be("Establishment status");

        // homePage.GetEstablishmentStatusFiltersByValueToLabel() => List<KeyValuePairs<ValueOfCheckbox, LabelOfTheCheckbox>>

        /*        var something = new[]
                {
                    new KeyValuePair<string, string>("Open", $"Open({})")
                }
                homePage.GetEstablishmentStatusFilters().Should().Be(something);*/
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

public class HttpRequestBuilder
{
    private string? _path = Routes.HOME;
    private List<KeyValuePair<string, string>> _query = new();

    public HttpRequestBuilder()
    {
    }

    public static HttpRequestBuilder Create() => new();

    public HttpRequestBuilder SetPath(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        _path = path;
        return this;
    }

    public HttpRequestBuilder AddQuery(KeyValuePair<string, string> query)
    {
        _query.Add(query);
        return this;
    }

    public HttpRequestMessage Build()
    {
        UriBuilder uri = new()
        {
            Path = _path,
            Query = _query.ToList()
            .Aggregate(new StringBuilder(), (init, queryPairs) => init.Append($"{queryPairs.Key}={queryPairs.Value}"))
                .ToString()
        };

        return new HttpRequestMessage()
        {
            RequestUri = uri.Uri
        };
    }
}
