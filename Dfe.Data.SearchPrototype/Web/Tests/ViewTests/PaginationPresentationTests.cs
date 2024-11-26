using AngleSharp;
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

    public PaginationPresentationTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task Pagination_CurrentPageIs1_ShowsCorrectPaginationElements()
    {
        //The result of this test should create the following sequence: 1,2,3,4,5 ... 8 next>>
        //
        // arrange
        const int CurrentPageNumber = 1;

        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWith(establishmentResultsCount: 76);
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
        IElement? previousPageButton = previousPageListItem?.Children.First(element => element.Id == "previous-page");
        previousPageButton?.TextContent.Should().Contain("previous");

        // first page number
        IHtmlListItemElement? firstPageNumberListItem = paginationListItems[1] as IHtmlListItemElement;
        firstPageNumberListItem?.ClassName.Should().Contain("govuk-visually-hidden");
        IElement? firstPageButton = firstPageNumberListItem?.Children.First(element => element.Id == "first-page");
        firstPageButton?.TextContent.Should().Contain("1");

        // previous ellipsis
        IHtmlListItemElement? previousEllipsisListItem = paginationListItems[2] as IHtmlListItemElement;
        previousEllipsisListItem?.ClassName.Should().Contain("govuk-visually-hidden");

        // button 1, current page 
        IHtmlListItemElement? pageNumber1ListItem = paginationListItems[3] as IHtmlListItemElement;
        pageNumber1ListItem?.ClassName.Should().Contain("govuk-pagination__item--current");
        IElement? currentPageLabelElement = pageNumber1ListItem?.Children.First(element => element.Id == $"pageNumber-{CurrentPageNumber}");
        currentPageLabelElement?.TextContent.Should().Contain($"{CurrentPageNumber}");

        // button 2
        IHtmlListItemElement? pageNumber2ListItem = paginationListItems[4] as IHtmlListItemElement;
        IElement? page2Button = pageNumber2ListItem?.Children.First(element => element.Id == "pageNumber-2");
        page2Button?.TextContent.Should().Contain("2");

        // button 3
        IHtmlListItemElement? pageNumber3ListItem = paginationListItems[5] as IHtmlListItemElement;
        IElement? page3Button = pageNumber3ListItem?.Children.First(element => element.Id == "pageNumber-3");
        page3Button?.TextContent.Should().Contain("3");

        // button 4
        IHtmlListItemElement? pageNumber4ListItem = paginationListItems[6] as IHtmlListItemElement;
        IElement? page4Button = pageNumber4ListItem?.Children.First(element => element.Id == "pageNumber-4");
        page4Button?.TextContent.Should().Contain("4");

        // button 5
        IHtmlListItemElement? pageNumber5ListItem = paginationListItems[7] as IHtmlListItemElement;
        IElement? page5Button = pageNumber5ListItem?.Children.First(element => element.Id == "pageNumber-5");
        page5Button?.TextContent.Should().Contain("5");

        // next ellipsis
        IHtmlListItemElement? nextEllipsisListItem = paginationListItems[8] as IHtmlListItemElement;
        nextEllipsisListItem?.ClassName.Should().NotContain("govuk-visually-hidden");
        nextEllipsisListItem?.ClassName.Should().Contain("govuk-pagination__item govuk-pagination__item--ellipses");

        // last page number, total page number
        IHtmlListItemElement? lastPageNumberListItem = paginationListItems[9] as IHtmlListItemElement;
        lastPageNumberListItem?.ClassName.Should().NotContain("govuk-visually-hidden");
        IElement? lastPageButton = lastPageNumberListItem?.Children.First(element => element.Id == "pageNumber-8");
        lastPageButton?.TextContent.Should().Contain("8");

        //next button
        IHtmlListItemElement? nextPageListItem = paginationListItems[10] as IHtmlListItemElement;
        nextPageListItem?.ClassName.Should().NotContain("govuk-visually-hidden");
        IElement? nextPageButton = nextPageListItem?.Children.First(element => element.Id == "next-page");
        nextPageButton?.TextContent.Should().Contain("next");
    }

    [Fact]
    public async Task Pagination_HasMoreLowerAndUpperPagesAvailable_ShowsCorrectPaginationElements()
    {
        //The result of this test should create the following sequence: <<previous 1 ... 19,20,21,22,23 ... 31 next>>
        //
        // arrange
        const int CurrentPageNumber = 21;

        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWith(establishmentResultsCount: 302);
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
        previousPageListItem?.ClassName.Should().NotContain("govuk-visually-hidden");
        IElement? previousPageButton = previousPageListItem?.Children.First(element => element.Id == "previous-page");
        previousPageButton?.TextContent.Should().Contain("previous");

        // first page number
        IHtmlListItemElement? firstPageNumberListItem = paginationListItems[1] as IHtmlListItemElement;
        firstPageNumberListItem?.ClassName.Should().NotContain("govuk-visually-hidden");
        IElement? firstPageButton = firstPageNumberListItem?.Children.First(element => element.Id == "first-page");
        firstPageButton?.TextContent.Should().Contain("1");

        // previous ellipsis
        IHtmlListItemElement? previousEllipsisListItem = paginationListItems[2] as IHtmlListItemElement;
        previousEllipsisListItem?.ClassName.Should().NotContain("govuk-visually-hidden");

        // button 19 
        IHtmlListItemElement? pageNumber19ListItem = paginationListItems[3] as IHtmlListItemElement;
        IElement? page19Button = pageNumber19ListItem?.Children.First(element => element.Id == "pageNumber-19");
        page19Button?.TextContent.Should().Contain("19");

        // button 20
        IHtmlListItemElement? pageNumber20ListItem = paginationListItems[4] as IHtmlListItemElement;
        IElement? page20Button = pageNumber20ListItem?.Children.First(element => element.Id == "pageNumber-20");
        page20Button?.TextContent.Should().Contain("20");

        // button 21, current page 
        IHtmlListItemElement? pageNumber21ListItem = paginationListItems[5] as IHtmlListItemElement;
        pageNumber21ListItem?.ClassName.Should().Contain("govuk-pagination__item--current");
        IElement? currentPageLabelElement = pageNumber21ListItem?.Children.First(element => element.Id == $"pageNumber-{CurrentPageNumber}");
        currentPageLabelElement?.TextContent.Should().Contain($"{CurrentPageNumber}");

        // button 22
        IHtmlListItemElement? pageNumber22ListItem = paginationListItems[6] as IHtmlListItemElement;
        IElement? page22Button = pageNumber22ListItem?.Children.First(element => element.Id == "pageNumber-22");
        page22Button?.TextContent.Should().Contain("22");

        // button 23
        IHtmlListItemElement? pageNumber23ListItem = paginationListItems[7] as IHtmlListItemElement;
        IElement? page23Button = pageNumber23ListItem?.Children.First(element => element.Id == "pageNumber-23");
        page23Button?.TextContent.Should().Contain("23");

        // next ellipsis
        IHtmlListItemElement? nextEllipsisListItem = paginationListItems[8] as IHtmlListItemElement;
        nextEllipsisListItem?.ClassName.Should().NotContain("govuk-visually-hidden");

        // last page number , total page number
        IHtmlListItemElement? lastPageNumberListItem = paginationListItems[9] as IHtmlListItemElement;
        lastPageNumberListItem?.ClassName.Should().NotContain("govuk-visually-hidden");
        IElement? lastPageButton = lastPageNumberListItem?.Children.First(element => element.Id == "pageNumber-31");
        lastPageButton?.TextContent.Should().Contain("31");

        //next button
        IHtmlListItemElement? nextPageListItem = paginationListItems[10] as IHtmlListItemElement;
        nextPageListItem?.ClassName.Should().NotContain("govuk-visually-hidden");
        IElement? nextPageButton = nextPageListItem?.Children.First(element => element.Id == "next-page");
        nextPageButton?.TextContent.Should().Contain("next");
    }

    [Fact]
    public async Task Pagination_HasNoMoreUpperPagesAvailable_ShowsCorrectPaginationElements()
    {
        //The result of this test should create the following sequence: <<previous 1 ... 12 13 14 15 16
        //
        // arrange
        const int CurrentPageNumber = 15;

        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWith(establishmentResultsCount: 157);
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
        previousPageListItem?.ClassName.Should().NotContain("govuk-visually-hidden");
        IElement? previousPageButton = previousPageListItem?.Children.First(element => element.Id == "previous-page");
        previousPageButton?.TextContent.Should().Contain("previous");

        // first page number
        IHtmlListItemElement? firstPageNumberListItem = paginationListItems[1] as IHtmlListItemElement;
        firstPageNumberListItem?.ClassName.Should().NotContain("govuk-visually-hidden");
        IElement? firstPageButton = firstPageNumberListItem?.Children.First(element => element.Id == "first-page");
        firstPageButton?.TextContent.Should().Contain("1");

        // previous ellipsis
        IHtmlListItemElement? previousEllipsisListItem = paginationListItems[2] as IHtmlListItemElement;
        previousEllipsisListItem?.ClassName.Should().NotContain("govuk-visually-hidden");

        // button 12 
        IHtmlListItemElement? pageNumber12ListItem = paginationListItems[3] as IHtmlListItemElement;
        IElement? page12Button = pageNumber12ListItem?.Children.First(element => element.Id == "pageNumber-12");
        page12Button?.TextContent.Should().Contain("12");

        // button 13
        IHtmlListItemElement? pageNumber13ListItem = paginationListItems[4] as IHtmlListItemElement;
        IElement? page13Button = pageNumber13ListItem?.Children.First(element => element.Id == "pageNumber-13");
        page13Button?.TextContent.Should().Contain("13");

        // button 14
        IHtmlListItemElement? pageNumber14ListItem = paginationListItems[5] as IHtmlListItemElement;
        IElement? page14Button = pageNumber14ListItem?.Children.First(element => element.Id == "pageNumber-14");
        page14Button?.TextContent.Should().Contain("14");

        // button 15, current page number
        IHtmlListItemElement? pageNumber15ListItem = paginationListItems[6] as IHtmlListItemElement;
        pageNumber15ListItem?.ClassName.Should().Contain("govuk-pagination__item--current");
        IElement? currentPageLabelElement = pageNumber15ListItem?.Children.First(element => element.Id == $"pageNumber-{CurrentPageNumber}");
        currentPageLabelElement?.TextContent.Should().Contain($"{CurrentPageNumber}");

        // button 16
        IHtmlListItemElement? pageNumber16ListItem = paginationListItems[7] as IHtmlListItemElement;
        IElement? page16Button = pageNumber16ListItem?.Children.First(element => element.Id == "pageNumber-16");
        page16Button?.TextContent.Should().Contain("16");

        // next ellipsis
        IHtmlListItemElement? nextEllipsisListItem = paginationListItems[8] as IHtmlListItemElement;
        nextEllipsisListItem?.ClassName.Should().Contain("govuk-visually-hidden");

        // last page number , total page number
        IHtmlListItemElement? lastPageNumberListItem = paginationListItems[9] as IHtmlListItemElement;
        lastPageNumberListItem?.ClassName.Should().Contain("govuk-visually-hidden");
        IElement? lastPageButton = lastPageNumberListItem?.Children.First(element => element.Id == "pageNumber-16");
        lastPageButton?.TextContent.Should().Contain("16");

        //next button
        IHtmlListItemElement? nextPageListItem = paginationListItems[10] as IHtmlListItemElement;
        nextPageListItem?.ClassName.Should().NotContain("govuk-visually-hidden");
        IElement? nextPageButton = nextPageListItem?.Children.First(element => element.Id == "next-page");
        nextPageButton?.TextContent.Should().Contain("next");
    }

    [Fact]
    public async Task Pagination_TotalPageNumberLessThan5_ShowsCorrectPaginationElements()
    {
        //The result of this test should create the following sequence: 1 2 3
        //
        // arrange
        const int CurrentPageNumber = 2;

        var useCaseResponse = SearchByKeywordResponseTestDouble.CreateWith(establishmentResultsCount: 24);
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
        previousPageListItem?.ClassName.Should().NotContain("govuk-visually-hidden");
        IElement? previousPageButton = previousPageListItem?.Children.First(element => element.Id == "previous-page");
        previousPageButton?.TextContent.Should().Contain("previous");

        // first page number
        IHtmlListItemElement? firstPageNumberListItem = paginationListItems[1] as IHtmlListItemElement;
        firstPageNumberListItem?.ClassName.Should().Contain("govuk-visually-hidden");
        IElement? firstPageButton = firstPageNumberListItem?.Children.First(element => element.Id == "first-page");
        firstPageButton?.TextContent.Should().Contain("1");

        // previous ellipsis
        IHtmlListItemElement? previousEllipsisListItem = paginationListItems[2] as IHtmlListItemElement;
        previousEllipsisListItem?.ClassName.Should().Contain("govuk-visually-hidden");

        // button 1
        IHtmlListItemElement? pageNumber1ListItem = paginationListItems[3] as IHtmlListItemElement;
        IElement? page1Button = pageNumber1ListItem?.Children.First(element => element.Id == "pageNumber-1");
        page1Button?.TextContent.Should().Contain("1");

        // button 2- current page
        IHtmlListItemElement? pageNumber2ListItem = paginationListItems[4] as IHtmlListItemElement;
        IElement? currentPageButton = pageNumber2ListItem?.Children.First(element => element.Id == $"pageNumber-{CurrentPageNumber}");
        currentPageButton?.TextContent.Should().Contain($"{CurrentPageNumber}");

        // button 3
        IHtmlListItemElement? pageNumber3ListItem = paginationListItems[5] as IHtmlListItemElement;
        IElement? page3Button = pageNumber3ListItem?.Children.First(element => element.Id == "pageNumber-3");
        page3Button?.TextContent.Should().Contain("3");

        // next ellipsis
        IHtmlListItemElement? nextEllipsisListItem = paginationListItems[6] as IHtmlListItemElement;
        nextEllipsisListItem?.ClassName.Should().Contain("govuk-visually-hidden");

        // last page number , total page number
        IHtmlListItemElement? lastPageNumberListItem = paginationListItems[7] as IHtmlListItemElement;
        lastPageNumberListItem?.ClassName.Should().Contain("govuk-visually-hidden");
        IElement? lastPageButton = lastPageNumberListItem?.Children.First(element => element.Id == "pageNumber-3");
        lastPageButton?.TextContent.Should().Contain("3");

        //next button
        IHtmlListItemElement? nextPageListItem = paginationListItems[8] as IHtmlListItemElement;
        nextPageListItem?.ClassName.Should().NotContain("govuk-visually-hidden");
        IElement? nextPageButton = nextPageListItem?.Children.First(element => element.Id == "next-page");
        nextPageButton?.TextContent.Should().Contain("next");
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
    public async Task Pagination_With1PageOfResults_NoPaginationComponentDisplayed()
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
}