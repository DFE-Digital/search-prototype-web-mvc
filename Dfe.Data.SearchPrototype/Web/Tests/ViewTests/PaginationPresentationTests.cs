﻿using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Dfe.Data.SearchPrototype.Web.Tests.ViewTests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.PresentationLayerTests;

public class PaginationPresentationTests : SharedTestFixture
{
    private const string homeUri = "http://localhost";

    public PaginationPresentationTests(WebApplicationFactory<Program> factory) :base(factory){
    }

    [Fact]
    public async Task Pagination_WithCurrentPageInLowerPaddingBoundry_ShowsCorrectPaginationElements()
    {
        //The result of this test should create the following sequence: 1,2,3,4,5 ... 8 next>>
        //
        // arrange
        const int CurrentPageNumber = 1;

        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWith(establishmentResultsCount: 76, pageNumber: CurrentPageNumber);
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        var resultsPage = await _context.OpenAsync($"{homeUri}?searchKeyword=anything&PageNumber={CurrentPageNumber}");

        // assert
        resultsPage.QuerySelector(HomePage.PaginationContainer.Criteria)!
            .Should().NotBeNull();

        // this gives us all list items we need
        List<IElement> paginationListItems =
             resultsPage.QuerySelector(HomePage.PaginationContainer.Criteria)!
                .GetMultipleElements(HomePage.PageNumberLinks.Criteria).ToList();
        //previous button
        IHtmlListItemElement? previousPageListItem = paginationListItems[0] as IHtmlListItemElement;
        previousPageListItem?.ClassName.Should().Contain("govuk-visually-hidden");
        IElement? previousPageButton = previousPageListItem?.Children.First(element => element.Id == "previous");
        previousPageButton?.TextContent.Should().Contain("previous");

        // previous ellipsis
        IHtmlListItemElement? previousEllipsisListItem = paginationListItems[1] as IHtmlListItemElement;
        previousEllipsisListItem?.ClassName.Should().Contain("govuk-visually-hidden");


        // GetElementById($"pageNumber-{pageNumber}")
    }

    [Fact]
    public async Task Pagination_WithNoResults_NoPaginationComponentDisplayed()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithNoResults();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        var resultsPage = await _context.OpenAsync($"{homeUri}?searchKeyWord=anything");

        // assert
        resultsPage.QuerySelector(HomePage.PaginationContainer.Criteria)!
            .Should().BeNull();
    }

    [Fact]
    public async Task Pagination_WithWith1PageOfResults_NoPaginationComponentDisplayed()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithOneResult();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        var resultsPage = await _context.OpenAsync($"{homeUri}?searchKeyword=anything");

        // assert
        resultsPage.QuerySelector(HomePage.PaginationContainer.Criteria)!
            .Should().BeNull();
    }


























    [Fact]
    public async Task Search_ByKeyword_WithSingleEstablishmentResult_Shows1ResultText()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWithOneResult();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        var resultsPage = await _context.OpenAsync($"{homeUri}?searchKeyword=anything");

        // assert
        resultsPage.QuerySelector(HomePage.SearchResultsNumber.Criteria)!
            .TextContent.Should().Be("1 of 1 result");
        resultsPage.QuerySelector(HomePage.SearchResultsContainer.Criteria)!
            .GetMultipleElements(HomePage.SearchResultLinks.Criteria)
            .Count().Should().Be(1);
    }

    

    [Fact]
    public async Task Search_ByKeyword_WithFacetedResults_ShowsFacets()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        var resultsPage = await _context.OpenAsync($"{homeUri}?searchKeyword=anything");

        // assert
        var filtersHeading = resultsPage.QuerySelector(HomePage.FiltersHeading.Criteria);
        filtersHeading.Should().NotBeNull();
        filtersHeading!.TextContent.Should().Be("Filters");

        var expectedFacets = useCaseResponse.EstablishmentFacetResults!.Facets;
        foreach (var expectedFacet in expectedFacets)
        {
            // get the page element and its child nodes for each expected facet - this bit could be done with a page model
            var matchingFacetPageElement = resultsPage.GetFacet(expectedFacet.Name);
            var facetLegend = matchingFacetPageElement.GetLegend();
            var facetInputElements = matchingFacetPageElement.GetCheckBoxes();

            // assert the facet name is on the page
            facetLegend.TextContent.Trim().Should().Be(expectedFacet.Name);
            foreach (var expectedFacetValue in expectedFacet.Results)
            {
                // assert that each expected facet value appears on the page under the correct facet name
                var matchedFacet = facetInputElements.Single(inputElement => inputElement!.Value == expectedFacetValue.Value);
                matchedFacet.Should().NotBeNull();
            }
        }
    }

    [Fact]
    public async Task UseCaseResponse_WithSelectedFacets_ShowsFacetsAsSelected()
    {
        // arrange
        var useCaseResponse = SearchByKeywordResponseTestDouble.Create();
        _useCase.Setup(useCase => useCase.HandleRequest(It.IsAny<SearchByKeywordRequest>()))
            .ReturnsAsync(useCaseResponse);

        // act
        var resultsPage = await _context.OpenAsync($"{homeUri}?searchKeyword=anything");
        var checkedBoxes = resultsPage.SelectFilters();
        IDocument filteredResultsPage = await resultsPage.SubmitSearchAsync();

        var resultsPageSelectedFacets = filteredResultsPage.GetFacets().SelectMany(facet => facet.GetCheckBoxes().Where(checkBox => checkBox.IsChecked));
        resultsPageSelectedFacets.Select(facet => facet.Value).Should().BeEquivalentTo(checkedBoxes.Select(facet => facet.Value));
    }
}