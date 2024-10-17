using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;

public static class SearchPageHelpers
{
    public static IEnumerable<IHtmlFieldSetElement> GetFacets(this IDocument searchPage)
    {
        var facetContainer = searchPage.QuerySelector(HomePage.FiltersContainer.Criteria);
        return facetContainer!
                .GetNodes<IHtmlFieldSetElement>();
    }
    public static IHtmlFieldSetElement GetFacet(this IDocument searchPage, string facetName)
    {
        return searchPage.GetFacets().Single(element => element.Id != null && element.Id == $"FacetName-{facetName}");
    }

    public static IHtmlLegendElement GetLegend(this IHtmlFieldSetElement facetFieldset) =>
        facetFieldset.GetNodes<IHtmlLegendElement>().Single();


    public static IEnumerable<IHtmlInputElement> GetCheckBoxes(this IHtmlFieldSetElement facetFieldset) =>
        facetFieldset.GetNodes<IHtmlInputElement>();

    public static IEnumerable<IHtmlInputElement> SelectFilters(this IDocument searchPage)
    {
        var firstFacetPageElement = searchPage.GetFacets().First();
        var facetInputElements = firstFacetPageElement.GetCheckBoxes();
        facetInputElements.First().IsChecked = true;
        return facetInputElements.Where(inputElement => inputElement.IsChecked);
    }

    public static void TypeIntoSearchBox(this IDocument searchPage, string content)
    {
        var searchBox = searchPage.QuerySelector(HomePage.SearchInput.Criteria) as IHtmlInputElement;
        searchBox!.Value = content;
    }

    public static Task<IDocument> SubmitSearchAsync(this IDocument searchPage)
    {
        IHtmlFormElement form = searchPage.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria)!;
        return form.SubmitAsync();
    }

    public static Task<IDocument> SubmitClearAsync(this IDocument searchPage)
    {
        IHtmlFormElement form = searchPage.QuerySelector<IHtmlFormElement>(HomePage.SearchForm.Criteria)!;

        IHtmlButtonElement? clearButton = searchPage.GetClearButton();

        string? buttonName = clearButton?.Name;
        string? buttonValue = clearButton?.Value;

        if (!string.IsNullOrWhiteSpace(buttonName) &&
            !string.IsNullOrWhiteSpace(buttonValue) &&
            clearButton != null)
        {
            clearButton.Name = $"{buttonName}_old";
            IHtmlInputElement input = searchPage.CreateElement<IHtmlInputElement>();
            input.Name = buttonName;
            input.Value = buttonValue;
            form.AppendChild(input);
        }
        return form.SubmitAsync();
    }

    public static IHtmlButtonElement? GetClearButton(this IDocument searchPage)
    {
        return searchPage?.QuerySelector<IHtmlButtonElement>(@"button[id=""clearFilters""]");
    }
}
