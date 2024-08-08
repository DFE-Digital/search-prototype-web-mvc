using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Data.SearchPrototype.Web.Tests.PageObjectModel;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Integration
{
    public class SearchPageTests : IClassFixture<PageWebApplicationFactory>
    {
        private const string uri = "http://localhost:5000";
        private readonly HttpClient _client; 
        private readonly ITestOutputHelper _logger;
        private readonly WebApplicationFactory<Program> _factory;

        public SearchPageTests(PageWebApplicationFactory factory, ITestOutputHelper logger)
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

            document.GetElementText(SearchPage.Heading.Criteria).Should().Be("Search prototype");
        }

        [Fact]
        public async Task Header_Link_IsDisplayed()
        {
            var response = await _client.GetAsync(uri);

            var document = await HtmlHelpers.GetDocumentAsync(response);

            document.GetElementText(SearchPage.HomeLink.Criteria).Should().Be("Home");
        }

        [Fact]
        public async Task Search_Establishment_IsDisplayed() 
        {
            var response = await _client.GetAsync(uri);

            var document = await HtmlHelpers.GetDocumentAsync(response);

            document.GetElementText(SearchPage.SearchHeading.Criteria).Should().Be("Search");
            
            document.GetElementText(SearchPage.SearchSubHeading.Criteria).Should().Be("Search establishments");
            
            document.GetMultipleElements(SearchPage.SearchInput.Criteria).Count().Should().Be(1);
            
            document.GetMultipleElements(SearchPage.SearchButton.Criteria).Count().Should().Be(1);
        }

        [Fact]
        public async Task Search_ByName_ReturnsSingleResult()
        {
            var response = await _client.GetAsync(uri);
            var document = await HtmlHelpers.GetDocumentAsync(response);

            var formElement = document.QuerySelector<IHtmlFormElement>(SearchPage.SearchForm.Criteria) ?? throw new Exception("Unable to find the search form");
            var formButton = document.QuerySelector<IHtmlButtonElement>(SearchPage.SearchButton.Criteria) ?? throw new Exception("Unable to find the submit button on search form");

            var formResponse = await _client.SendAsync(
                formElement,
                formButton,
                new Dictionary<string, string>
                {
                    ["searchKeyWord"] = "School"
                });

            _logger.WriteLine("SendAsync client base address: " + _client.BaseAddress);
            _logger.WriteLine("SendAsync request message: " + formResponse.RequestMessage!.ToString());

            var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

            _logger.WriteLine("Document: " + resultsPage.Body!.OuterHtml);

            resultsPage.GetElementText(SearchPage.SearchResultsNumber.Criteria).Should().Contain("Result");
            resultsPage.GetMultipleElements(SearchPage.SearchResultLinks.Criteria).Count().Should().Be(1);
        }
        
        [Fact]
        public async Task Search_ByName_ReturnsMultipleResults()
        {
            var response = await _client.GetAsync(uri);
            var document = await HtmlHelpers.GetDocumentAsync(response);

            var formElement = document.QuerySelector<IHtmlFormElement>(SearchPage.SearchForm.Criteria) ?? throw new Exception("Unable to find the sign in form");
            var formButton = document.QuerySelector<IHtmlButtonElement>(SearchPage.SearchButton.Criteria) ?? throw new Exception("Unable to find the submit button on search form");

            var formResponse = await _client.SendAsync(
                formElement,
                formButton,
                new Dictionary<string, string>
                {
                    ["searchKeyWord"] = "Academy"
                });

            _logger.WriteLine("SendAsync client base address: " + _client.BaseAddress);
            _logger.WriteLine("SendAsync request message: " + formResponse.RequestMessage!.ToString());

            var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

            _logger.WriteLine("Document: " + resultsPage.Body!.OuterHtml);

            resultsPage.GetElementText(SearchPage.SearchResultsNumber.Criteria).Should().Contain("Results");
            resultsPage.GetMultipleElements(SearchPage.SearchResultLinks.Criteria).Count().Should().BeGreaterThan(1);
        }
        
        [Theory]
        [InlineData("ant")]
        [InlineData("boo")]
        public async Task Search_ByName_NoMatch_ReturnsNoResults(string searchTerm)
        {
            var response = await _client.GetAsync(uri);
            var document = await HtmlHelpers.GetDocumentAsync(response);

            var formElement = document.QuerySelector<IHtmlFormElement>(SearchPage.SearchForm.Criteria) ?? throw new Exception("Unable to find the sign in form");
            var formButton = document.QuerySelector<IHtmlButtonElement>(SearchPage.SearchButton.Criteria) ?? throw new Exception("Unable to find the submit button on search form");

            var formResponse = await _client.SendAsync(
                formElement,
                formButton,
                new Dictionary<string, string>
                {
                    ["searchKeyWord"] = searchTerm
                });

            _logger.WriteLine("SendAsync client base address: " + _client.BaseAddress);
            _logger.WriteLine("SendAsync request message: " + formResponse.RequestMessage!.ToString());

            var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

            _logger.WriteLine("Document: " + resultsPage.Body!.OuterHtml);

            resultsPage.GetElementText(SearchPage.SearchNoResultText.Criteria).Should().Be("Sorry no results found please amend your search criteria");
        }
    }
}
