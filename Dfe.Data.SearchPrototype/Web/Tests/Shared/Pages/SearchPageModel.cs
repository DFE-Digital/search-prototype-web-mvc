using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;

namespace Dfe.Data.SearchPrototype.Web.Tests.PresentationLayerTests;

public class SearchPageModel
{
    private SearchPageModelBuilder _builder;

    //private const string SearchResultsContainerDefinition = "#results";
    //private const string FilterContainerDefinition = "#filters-container";
    private const string homeUri = "http://localhost";

    public SearchResults? Results { get; set; }
    public SearchForm? Form { get; set; }
    public SearchFilterSection? FilterSection { get; set; }

    public string? ResultsText { get => Results?.ResultsText; }

    private IDocument? _dom;
    private IBrowsingContext _context;

    public SearchPageModel(IBrowsingContext context)
    {
        _context = context;
    }

    public async Task NavigateToPage(string uri)
    {
        _dom = await _context.OpenAsync(uri);
        Results = new SearchResults(_dom!);
        Form = new SearchForm(_dom!);
        FilterSection = _builder.FilterContainer() != null ? new SearchFilterSection(_builder.FilterContainer()!) : null;
    }

    public class SearchResults
    {
        private const string SearchResultsTextDefinition = "#search-results-count";
        private const string SearchResultLinksDefinition = "ul li h4";
        private const string SearchNoResultTextDefinition = "#no-results";

        public string? ResultsText { get; set; }
        public string? NoResultsText { get; set; }
        public IEnumerable<IElement>? SearchResultLinks { get; set; }

        public SearchResults(IDocument baseElement)
        {
            ResultsText = baseElement.QuerySelector(SearchResultsTextDefinition)?.TextContent;
            SearchResultLinks = baseElement.GetMultipleElements(HomePage.SearchResultLinks.Criteria);
            NoResultsText = baseElement.QuerySelector(SearchNoResultTextDefinition)?.TextContent;
        }
    }

    public class SearchForm
    {
        private const string SearchInputDefinition = "#searchKeyWord";
        private const string SearchFormDefinition = "#search-establishments-form";
        private const string SearchButtonDefinition = "#search-establishments button";

        private IDocument _dom;
        private IElement? InputTextBox;
        private IHtmlButtonElement? SubmitButton;
        private IHtmlButtonElement? ClearButton;
        private IHtmlFormElement? Form;

        public SearchForm(IDocument dom)
        {
            InputTextBox = dom.QuerySelector(SearchInputDefinition);
            SubmitButton = dom.QuerySelector<IHtmlButtonElement>(SearchButtonDefinition);
            ClearButton = dom.QuerySelector<IHtmlButtonElement>(@"button[id=""clearFilters""]");
            Form = dom.QuerySelector<IHtmlFormElement>(SearchFormDefinition);
            _dom = dom; // stored so that we can simulate the button presses by adding elements to the DOM 
        }

        public Task SubmitAsync()
        {
            return Form!.SubmitAsync();
        }

        public Task ClearButtonSubmitAsync()
        {
            string? buttonName = ClearButton?.Name;
            string? buttonValue = ClearButton?.Value;

            if (!string.IsNullOrWhiteSpace(buttonName) &&
                !string.IsNullOrWhiteSpace(buttonValue) &&
                ClearButton != null)
            {
                // simulates the button press by manually adding the new element to the page
                // - TODO - reference an explanation of what's happening or does Aasim do it a better way - getting the attributes from the button?
                ClearButton.Name = $"{buttonName}_old";
                IHtmlInputElement input = _dom.CreateElement<IHtmlInputElement>();
                input.Name = buttonName;
                input.Value = buttonValue;
                Form!.AppendChild(input);
            }
            return Form!.SubmitAsync();
        }
    }

    public class SearchFilterSection // the entire container section that contains all the filters
    {
        private const string FiltersHeadingDefinition = "#filters-heading";
        private IEnumerable<IHtmlFieldSetElement> _filters;
        private IElement _filterContainer;

        public SearchFilterSection(IElement element)
        {
            _filterContainer = element;
            _filters = element.GetNodes<IHtmlFieldSetElement>();
            FiltersHeading = element.QuerySelector(FiltersHeadingDefinition)?.TextContent;
            Filters = _filters.Select(filter => new Filter(filter));
        }

        public string? FiltersHeading { get; }
        public IEnumerable<Filter> Filters { get; }


        public Filter GetFilterByName(string filterName) => Filters.Where(filter => filter.Name == filterName).Single();

        public void SelectFilters(Dictionary<string, string> filters) // key = filter name, value = filter value
        {
            foreach (var filter in filters)
            {
                GetFilterByName(filter.Key).SelectCheckBoxWithValue(filter.Value);
            }
        }
    }

    /// <summary>
    /// Contains everything for one filter option: the filter name, its possible values and checkboxes
    /// </summary>
    public class Filter
    {
        private IEnumerable<IHtmlInputElement> _checkBoxes;
        private IHtmlFieldSetElement _filter;

        public Filter(IHtmlFieldSetElement filter)
        {
            _filter = filter;
            Name = filter.GetNodes<IHtmlLegendElement>().Single().TextContent.Trim();
            _checkBoxes = filter.GetNodes<IHtmlInputElement>();
        }

        public string Name { get; }
        public IEnumerable<string> CheckBoxValues => _checkBoxes.Select(checkbox => checkbox.Value);
        public IEnumerable<string> SelectedCheckBoxValues => _checkBoxes.Where(checkBox => checkBox.IsChecked).Select(checkBox => checkBox.Value);

        public void SelectCheckBoxWithValue(string value) =>
            _checkBoxes.Where(checkbox => checkbox.Value == value).Single().IsChecked = true;
    }
}