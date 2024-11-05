using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;

public class AngleSharpTinySearchPageBuilder
{
    private IBrowsingContext _context;
    private IDocument? _dom;

    public AngleSharpTinySearchPageBuilder(IBrowsingContext context)
    {
        _context = context;
    }

    // ------- get elements from the page
    public string? MainHeading() => _dom!.QuerySelector("#page-heading")?.TextContent;
    public string? NoResultsText() => _dom!.QuerySelector("#no-results")?.TextContent;
    public string? ResultsText() => _dom!.QuerySelector("#search-results-count")?.TextContent;
    public string? FilterSectionHeading() => _dom!.QuerySelector("#filters-heading")?.TextContent;
    public IEnumerable<KeyValuePair<string, string>>? Filters()
    {
        return _dom!.QuerySelector("#filters-container")?
            .GetNodes<IHtmlFieldSetElement>()
            .SelectMany(
                element => element
                    .GetNodes<IHtmlInputElement>()
                    .Select(node => new KeyValuePair<string, string>(
                        element.GetNodes<IHtmlLegendElement>().Single().InnerHtml.Trim(),
                        node.Value)));
    }

    public IEnumerable<KeyValuePair<string, string>>? SelectedFilters()
    {
        return _dom!.QuerySelector("#filters-container")?
            .GetNodes<IHtmlFieldSetElement>()
            .SelectMany(
                element => element
                    .GetNodes<IHtmlInputElement>()
                    .Where(element => element.IsChecked == true)
                    .Select(node => new KeyValuePair<string, string>(
                        element.GetNodes<IHtmlLegendElement>().Single().InnerHtml.Trim(),
                        node.Value)));
    }

    //------- equality cases for test asserts
    public bool FilterSectionIsNullOrEmpty() => _dom!.QuerySelector("#filters-container") == null;

    //------- actions
    public async Task NavigateToPage(string uri)
    {
        _dom = await _context.OpenAsync(uri);
    }

    public Task SubmitAsync()
    {
        var form = _dom!.QuerySelector<IHtmlFormElement>("#search-establishments-form");
        return form!.SubmitAsync();
    }

    public Task SubmitClearAsync()
    {
        var clearButton = _dom!.QuerySelector<IHtmlButtonElement>(@"button[id=""clearFilters""]");
        var form = _dom!.QuerySelector<IHtmlFormElement>("#search-establishments-form");
        string? buttonName = clearButton?.Name;
        string? buttonValue = clearButton?.Value;

        if (!string.IsNullOrWhiteSpace(buttonName) &&
            !string.IsNullOrWhiteSpace(buttonValue) &&
            clearButton != null)
        {
            // simulates the button press by manually adding the new element to the page
            // - TODO - reference an explanation of what's happening or does Aasim do it a better way?
            clearButton.Name = $"{buttonName}_old";
            IHtmlInputElement input = _dom!.CreateElement<IHtmlInputElement>();
            input.Name = buttonName;
            input.Value = buttonValue;
            form!.AppendChild(input);
        }
        return form!.SubmitAsync();
    }

    public void SelectFilters(Dictionary<string, string> filters) // key = filter name, value = filter value
    {
        foreach (var filter in filters)
        {
            var filterBoxesForFilterName = GetFilterByName(filter.Key);
            var checkBox = filterBoxesForFilterName?.GetNodes<IHtmlInputElement>()
                .Where(checkbox => checkbox.Value == filter.Value)
                .Single();
            checkBox!.IsChecked = true;
        }
    }

    private IHtmlFieldSetElement? GetFilterByName(string filterName)
    {
        return _dom!.QuerySelector("#filters-container")?
            .GetNodes<IHtmlFieldSetElement>()
            .Where(element => element.GetNodes<IHtmlLegendElement>().Single().InnerHtml.Trim() == filterName).Single();
    }
}
