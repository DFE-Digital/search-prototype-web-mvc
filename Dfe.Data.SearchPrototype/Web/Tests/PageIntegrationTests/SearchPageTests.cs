﻿using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Data.SearchPrototype.Web.Tests.Pages.Search;
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

            var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

            resultsPage.GetElementText(SearchResultsPage.SearchResultsNumber.Criteria).Should().Contain("Result");
            resultsPage.GetMultipleElements(SearchResultsPage.SearchResultLinks.Criteria).Count().Should().Be(1);
            resultsPage.GetElementText(SearchResultsPage.FirstResultEstablishmentName.Criteria).Should().Be("Duck School");
            resultsPage.GetElementText(SearchResultsPage.FirstResultEstablishmentUrn.Criteria).Should().Contain("345678");
            resultsPage.GetElementText(SearchResultsPage.FirstResultEstablishmentAddress.Criteria).Should().Contain("Duck Street, Duck Locality, Duck Address 3, Duck Town, DUU CKK");
            resultsPage.GetElementText(SearchResultsPage.FirstResultEstablishmentType.Criteria).Should().Contain("Community School");
            resultsPage.GetElementText(SearchResultsPage.FirstResultEstablishmentStatus.Criteria).Should().Contain("Closed");
            resultsPage.GetElementText(SearchResultsPage.FirstResultEstablishmentPhase.Criteria).Should().Contain("Primary");
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

            var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

            resultsPage.GetElementText(SearchResultsPage.SearchResultsNumber.Criteria).Should().Contain("Results");
            resultsPage.GetMultipleElements(SearchResultsPage.SearchResultLinks.Criteria).Count().Should().Be(2);
            
            resultsPage.GetElementText(SearchResultsPage.FirstResultEstablishmentName.Criteria).Should().Be("Goose Academy");
            resultsPage.GetElementText(SearchResultsPage.FirstResultEstablishmentUrn.Criteria).Should().Contain("123456");
            resultsPage.GetElementText(SearchResultsPage.FirstResultEstablishmentAddress.Criteria).Should().Contain("Goose Street, Goose Locality, Goose Address 3, Goose Town, GOO OSE");
            resultsPage.GetElementText(SearchResultsPage.FirstResultEstablishmentType.Criteria).Should().Contain("Academy");
            resultsPage.GetElementText(SearchResultsPage.FirstResultEstablishmentStatus.Criteria).Should().Contain("Closed");
            resultsPage.GetElementText(SearchResultsPage.FirstResultEstablishmentPhase.Criteria).Should().Contain("Primary");
            
            resultsPage.GetElementText(SearchResultsPage.SecondResultEstablishmentName.Criteria).Should().Be("Horse Academy");
            resultsPage.GetElementText(SearchResultsPage.SecondResultEstablishmentUrn.Criteria).Should().Contain("234567");
            resultsPage.GetElementText(SearchResultsPage.SecondResultEstablishmentAddress.Criteria).Should().Contain("Horse Street, Horse Locality, Horse Address 3, Horse Town, HOR SEE");
            resultsPage.GetElementText(SearchResultsPage.SecondResultEstablishmentType.Criteria).Should().Contain("Academy");
            resultsPage.GetElementText(SearchResultsPage.SecondResultEstablishmentStatus.Criteria).Should().Contain("Closed");
            resultsPage.GetElementText(SearchResultsPage.SecondResultEstablishmentPhase.Criteria).Should().Contain("Primary");
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

            var resultsPage = await HtmlHelpers.GetDocumentAsync(formResponse);

            resultsPage.GetElementText(SearchResultsPage.SearchNoResultText.Criteria).Should().Be("Sorry no results found please amend your search criteria");
        }
    }
}
