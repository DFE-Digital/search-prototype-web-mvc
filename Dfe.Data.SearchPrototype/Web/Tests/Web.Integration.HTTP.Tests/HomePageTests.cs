﻿using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V127.DOM;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests
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

            var document = await response.GetDocumentAsync();

            document.GetElementText(HomePage.Heading.Criteria).Should().Be("Search prototype");
        }

        [Fact]
        public async Task Header_Link_IsDisplayed()
        {
            var response = await _client.GetAsync(uri);

            var document = await response.GetDocumentAsync();

            document.GetElementText(HomePage.HomeLink.Criteria).Should().Be("Home");
        }

        [Fact]
        public async Task Search_Establishment_IsDisplayed()
        {
            var response = await _client.GetAsync(uri);

            var document = await response.GetDocumentAsync();

            document.GetElementText(HomePage.SearchHeading.Criteria).Should().Be("Search");

            document.GetElementText(HomePage.SearchSubHeading.Criteria).Should().Be("Search establishments");

            document.GetMultipleElements(HomePage.SearchInput.Criteria).Count().Should().Be(1);

            document.GetMultipleElements(HomePage.SearchButton.Criteria).Count().Should().Be(1);
        }

        [Fact]
        public async Task Search_ByName_Returns_LessThan100_Results()
        {
            var response = await _client.GetAsync(uri);
            var document = await response.GetDocumentAsync();

            var formElement = document.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria) ?? throw new Exception("Unable to find the search form");
            var formButton = document.QuerySelector<IHtmlButtonElement>(HomePage.SearchButton.Criteria) ?? throw new Exception("Unable to find the submit button on search form");

            var formResponse = await _client.SendAsync(
                formElement,
                formButton,
                new Dictionary<string, string>
                {
                    ["searchKeyWord"] = "One"
                });

            var resultsPage = await formResponse.GetDocumentAsync();

            var searchResultsNumber = resultsPage.GetElementText(HomePage.SearchResultsNumber.Criteria);
            searchResultsNumber.Should().Contain("Result");

            resultsPage.GetMultipleElements(HomePage.SearchResultsHeadings.Criteria).Count().Should().BeLessThan(100);
        }

        [Fact]
        public async Task Search_ByName_ReturnsMultipleResults()
        {
            var response = await _client.GetAsync(uri);
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
            var response = await _client.GetAsync(uri);
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
            var response = await _client.GetAsync(uri);
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

        [Fact]
        public async Task Filters_AreDisplayed()
        {
            var response = await _client.GetAsync(uri + "/?searchKeyWord=Academy");
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

        [Fact]
        public async Task Apply_Filters_Button()
        {
            var response = await _client.GetAsync(uri + "/?searchKeyWord=Middle");
            var document = await response.GetDocumentAsync();

            var filtersButton = document.QuerySelector("#filters-button")!.GetAttribute("type");
            filtersButton.Should().Be("submit");
        }

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

        public static IEnumerable<object[]> EstablishmentStatusElements()
        {
            yield return new object[] { "middle", "ESTABLISHMENTSTATUSNAME", "Open", HomePage.OpenFilterInput.Criteria, "Open" };
            yield return new object[] { "academy", "ESTABLISHMENTSTATUSNAME", "Closed", HomePage.ClosedFilterInput.Criteria, "Closed" };
            yield return new object[] { "school", "ESTABLISHMENTSTATUSNAME", "Proposed+to+open", HomePage.ProposedToOpenFilterInput.Criteria, "Proposed to open" };
            yield return new object[] { "isle", "ESTABLISHMENTSTATUSNAME", "Open%2C+but+proposed+to+close", HomePage.OpenProposedToCloseFilterInput.Criteria, "Open" };
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

        public static IEnumerable<object[]> PhaseOfEducationElements()
        {
            yield return new object[] { "west", "PHASEOFEDUCATION", "Primary", HomePage.PrimaryFilterInput.Criteria, "Primary" };
            yield return new object[] { "west", "PHASEOFEDUCATION", "Secondary", HomePage.SecondaryFilterInput.Criteria, "Secondary" };
            yield return new object[] { "west", "PHASEOFEDUCATION", "Not+applicable", HomePage.NAFilterInput.Criteria, "Not applicable" };
            yield return new object[] { "west", "PHASEOFEDUCATION", "All-through", HomePage.AllThroughFilterInput.Criteria, "All-through" };
            yield return new object[] { "west", "PHASEOFEDUCATION", "Middle+deemed+secondary", HomePage.MiddleDeemedSecondaryFilterInput.Criteria, "Middle deemed secondary" };
            yield return new object[] { "west", "PHASEOFEDUCATION", "16+plus", HomePage.SixteenPlusFilterInput.Criteria, "16 plus" };
            yield return new object[] { "west", "PHASEOFEDUCATION", "Middle+deemed+primary", HomePage.MiddleDeemedPrimaryFilterInput.Criteria, "Middle deemed primary" };
        }
    }
}
