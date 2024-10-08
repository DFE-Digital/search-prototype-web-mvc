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

            resultsPage.GetMultipleElements(HomePage.SearchResultLinks.Criteria).Count().Should().BeLessThan(100);
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
            resultsPage.GetMultipleElements(HomePage.SearchResultLinks.Criteria).Count().Should().Be(100);
        }

        [Theory]
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

            var establishmentStatusName = document.GetElementText(HomePage.EstablishmentStatusNameHeading.Criteria);
            establishmentStatusName.Should().Be("ESTABLISHMENT STATUS NAME");

            var phaseOfEducation = document.GetElementText(HomePage.PhaseOfEducationHeading.Criteria);
            phaseOfEducation.Should().Be("PHASE OF EDUCATION");
            
            var primaryInput = document.GetMultipleElements(HomePage.PrimaryFilterInput.Criteria);
            primaryInput.Should().HaveCount(1);
        }
    }
}
