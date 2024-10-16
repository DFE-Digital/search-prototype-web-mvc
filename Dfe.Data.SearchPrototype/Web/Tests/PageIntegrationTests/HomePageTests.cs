using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using Dfe.Data.SearchPrototype.Web.Tests.Shared;

namespace Dfe.Data.SearchPrototype.Web.Tests.Integration
{
    public class HomePageTests : IClassFixture<PageWebApplicationFactory<Program>>
    {
        private const string uri = "http://localhost:5000";
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _logger;
        private readonly WebApplicationFactory<Program> _factory;

        public HomePageTests(PageWebApplicationFactory<Program> factory, ITestOutputHelper logger)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true
            });
            _logger = logger;
        }

        [Fact]
        public async Task Search_Title_IsDisplayed()
        {
            var response = await _client.GetAsync(uri);

            var document = await HtmlHelpers.GetDocumentAsync(response);

            document.GetElementText(HomePage.Heading.Criteria).Should().Be("Search prototype");
        }

        [Fact]
        public async Task Header_Link_IsDisplayed()
        {
            var response = await _client.GetAsync(uri);

            var document = await HtmlHelpers.GetDocumentAsync(response);

            document.GetElementText(HomePage.HomeLink.Criteria).Should().Be("Home");
        }

        [Fact]
        public async Task Search_Establishment_IsDisplayed()
        {
            var response = await _client.GetAsync(uri);

            var document = await HtmlHelpers.GetDocumentAsync(response);

            document.GetElementText(HomePage.SearchHeading.Criteria).Should().Be("Search");

            document.GetElementText(HomePage.SearchSubHeading.Criteria).Should().Be("Search establishments");

            document.GetMultipleElements(HomePage.SearchInput.Criteria).Count().Should().Be(1);

            document.GetMultipleElements(HomePage.SearchButton.Criteria).Count().Should().Be(1);
        }

        [Fact]
        public async Task Search_ByName_Returns_LessThan100_Results()
        {
            var response = await _client.GetAsync(uri);
            var document = await HtmlHelpers.GetDocumentAsync(response);

            var formElement = document.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria) ?? throw new Exception("Unable to find the search form");
            var formButton = document.QuerySelector<IHtmlButtonElement>(HomePage.SearchButton.Criteria) ?? throw new Exception("Unable to find the submit button on search form");

            var formResponse = await _client.SendAsync(
                formElement,
                formButton,
                new Dictionary<string, string>
                {
                    ["searchKeyWord"] = "One"
                });

            var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

            var searchResultsNumber = resultsPage.GetElementText(HomePage.SearchResultsNumber.Criteria);
            searchResultsNumber.Should().Contain("Result");

            resultsPage.GetMultipleElements(HomePage.SearchResultsHeadings.Criteria).Count().Should().BeLessThan(100);
        }

        [Fact]
        public async Task Search_ByName_ReturnsMultipleResults()
        {
            var response = await _client.GetAsync(uri);
            var document = await HtmlHelpers.GetDocumentAsync(response);

            var formElement = document.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria) ?? throw new Exception("Unable to find the sign in form");
            var formButton = document.QuerySelector<IHtmlButtonElement>(HomePage.SearchButton.Criteria) ?? throw new Exception("Unable to find the submit button on search form");

            var formResponse = await _client.SendAsync(
                formElement,
                formButton,
                new Dictionary<string, string>
                {
                    ["searchKeyWord"] = "Academy"
                });

            var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

            resultsPage.GetElementText(HomePage.SearchResultsNumber.Criteria).Should().Contain("Results");
            resultsPage.GetMultipleElements(HomePage.SearchResultsHeadings.Criteria).Count().Should().Be(100);
        }
        
        [Theory]
        [InlineData("St")]
        [InlineData("Jos")]
        [InlineData("Cath")]
        public async Task Search_ByPartialName_ReturnsResults(string term)
        {
            var response = await _client.GetAsync(uri);
            var document = await HtmlHelpers.GetDocumentAsync(response);

            var formElement = document.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria) ?? throw new Exception("Unable to find the sign in form");
            var formButton = document.QuerySelector<IHtmlButtonElement>(HomePage.SearchButton.Criteria) ?? throw new Exception("Unable to find the submit button on search form");

            var formResponse = await _client.SendAsync(
                formElement,
                formButton,
                new Dictionary<string, string>
                {
                    ["searchKeyWord"] = term
                });

            var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

            var resultsNumber = resultsPage.GetElementText(HomePage.SearchResultsNumber.Criteria);
            resultsNumber.Should().Contain("Results");

            var resultsHeadingsText = resultsPage.GetMultipleElements(HomePage.SearchResultsHeadings.Criteria);
            resultsHeadingsText.Should().HaveCountGreaterThan(1);

            var resultsHeadings = resultsPage.QuerySelector(HomePage.SearchResultsHeadings.Criteria);
            foreach( var headings in resultsHeadings!.Text())
            {
                resultsHeadings!.TextContent.Should().ContainAny(term);
            }
            
        }

        [Theory]
        [InlineData("abcd")]
        [InlineData("zzz")]
        public async Task Search_ByName_NoMatch_ReturnsNoResults(string searchTerm)
        {
            var response = await _client.GetAsync(uri);
            var document = await HtmlHelpers.GetDocumentAsync(response);

            var formElement = document.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria) ?? throw new Exception("Unable to find the sign in form");
            var formButton = document.QuerySelector<IHtmlButtonElement>(HomePage.SearchButton.Criteria) ?? throw new Exception("Unable to find the submit button on search form");

            var formResponse = await _client.SendAsync(
                formElement,
                formButton,
                new Dictionary<string, string>
                {
                    ["searchKeyWord"] = searchTerm
                });

            var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

            var noResultText = resultsPage.GetElementText(HomePage.SearchNoResultText.Criteria);
            noResultText.Should().Contain("Sorry no results found please amend your search criteria");
        }

        [Fact]
        public async Task Filters_AreDisplayed()
        {
            var response = await _client.GetAsync(uri + "/?searchKeyWord=Academy");
            var document = await HtmlHelpers.GetDocumentAsync(response);

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
    }
}
