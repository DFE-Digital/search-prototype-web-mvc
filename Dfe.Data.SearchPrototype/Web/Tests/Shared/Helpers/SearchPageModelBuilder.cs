using AngleSharp.Dom;
using AngleSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;

public class SearchPageModelBuilder
{
    private IBrowsingContext _context;
    private IDocument? _dom;
    private const string SearchResultsContainerDefinition = "#results";
    private const string FilterContainerDefinition = "#filters-container";

    public SearchPageModelBuilder(IBrowsingContext context)
    {
        _context = context;
    }

    public string? MainHeading() => _dom!.QuerySelector("#page-heading")?.TextContent;
    public string? NoResultsText() => _dom!.QuerySelector("#no-results")?.TextContent;
    public string? ResultsText() => _dom!.QuerySelector("#search-results-count")?.TextContent;
    public string? FilterSectionHeading() => _dom!.QuerySelector("#filters-heading")?.TextContent;
    public IElement? FilterContainer() => _dom!.QuerySelector("#filters-container");



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
}
