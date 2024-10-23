using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using Dfe.Data.SearchPrototype.Web.Tests.Shared;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using DfE.Data.SearchPrototype.Pages;
using HomePage = DfE.Data.SearchPrototype.Pages.HomePage;


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
        public async Task Search_Title_Displayed()
        {
            // Act
            Response response = await _client.GetHttpResponseAsync("/");

            // Assert
            new HomePage(response.DomQueryClient).GetHeading().Should().Be("Search prototype");
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

            document.GetElementText(Shared.Pages.HomePage.SearchHeading.Criteria).Should().Be("Search");

            document.GetElementText(HomePage.SearchSubHeading.Criteria).Should().Be("Search establishments");

            document.GetMultipleElements(HomePage.SearchInput.Criteria).Count().Should().Be(1);

            document.GetMultipleElements(HomePage.SearchButton.Criteria).Count().Should().Be(1);
        }

        [Fact]
        public async Task Search_ByName_ReturnsSingleResult()
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
                    ["searchKeyWord"] = "School"
                });

            var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

            resultsPage.GetElementText(HomePage.SearchResultsNumber.Criteria).Should().Contain("Result");
            resultsPage.GetMultipleElements(HomePage.SearchResultLinks.Criteria).Count().Should().Be(1);

            resultsPage.GetElementText(HomePage.SearchResultEstablishmentName(Constants.Urns.DUCK_SCHOOL).Criteria).Should().Be("Duck School");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentUrn(Constants.Urns.DUCK_SCHOOL).Criteria).Should().Contain("345678");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentAddress(Constants.Urns.DUCK_SCHOOL).Criteria).Should().Contain("Duck Street, Duck Locality, Duck Address 3, Duck Town, DUU CKK");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentType(Constants.Urns.DUCK_SCHOOL).Criteria).Should().Contain("Community School");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentStatus(Constants.Urns.DUCK_SCHOOL).Criteria).Should().Contain("Open");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentPhase(Constants.Urns.DUCK_SCHOOL).Criteria).Should().Contain("Primary");
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
            resultsPage.GetMultipleElements(HomePage.SearchResultLinks.Criteria).Count().Should().Be(2);

            resultsPage.GetElementText(HomePage.SearchResultEstablishmentName(Constants.Urns.GOOSE_ACADEMY).Criteria).Should().Be("Goose Academy");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentUrn(Constants.Urns.GOOSE_ACADEMY).Criteria).Should().Contain("123456");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentAddress(Constants.Urns.GOOSE_ACADEMY).Criteria).Should().Contain("Goose Street, Goose Locality, Goose Address 3, Goose Town, GOO OSE");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentType(Constants.Urns.GOOSE_ACADEMY).Criteria).Should().Contain("Academy");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentStatus(Constants.Urns.GOOSE_ACADEMY).Criteria).Should().Contain("Open");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentPhase(Constants.Urns.GOOSE_ACADEMY).Criteria).Should().Contain("Secondary");
                                                         
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentName(Constants.Urns.HORSE_ACADEMY).Criteria).Should().Be("Horse Academy");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentUrn(Constants.Urns.HORSE_ACADEMY).Criteria).Should().Contain("234567");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentAddress(Constants.Urns.HORSE_ACADEMY).Criteria).Should().Contain("Horse Street, Horse Locality, Horse Address 3, Horse Town, HOR SEE");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentType(Constants.Urns.HORSE_ACADEMY).Criteria).Should().Contain("Academy");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentStatus(Constants.Urns.HORSE_ACADEMY).Criteria).Should().Contain("Open");
            resultsPage.GetElementText(HomePage.SearchResultEstablishmentPhase(Constants.Urns.HORSE_ACADEMY).Criteria).Should().Contain("Post 16");
        }

        [Theory]
        [InlineData("ant")]
        public async Task Search_ByName_NoMatch_ReturnsNoResults(string searchTerm)
        {
            var response = await _client.GetAsync(uri);
            var document = await HtmlHelpers.GetDocumentAsync(response);

            // using Anglesharp to get document elements
            var formElement = document.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria) ?? throw new Exception("Unable to find the sign in form");
            var formButton = document.QuerySelector<IHtmlButtonElement>(HomePage.SearchButton.Criteria) ?? throw new Exception("Unable to find the submit button on search form");

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

            // using the selenium selector under the hood
            var thingToTest = resultsPage.GetElementText(HomePage.SearchNoResultText.Criteria);
            thingToTest.Should().Contain("Sorry no results found please amend your search criteria");
        }
    }
}