using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;

namespace Dfe.Data.SearchPrototype.Web.Tests.PresentationLayerTests;

public class SearchPageModel
{
    private static string SearchResultsContainerDefinition => "#results";
    private static string FilterContainerDefinition => "#filters-container";

    public SearchResults? Results { get; set; }
    public SearchForm? Form { get; set; }
    public SearchFilterSection? FilterSection { get; set; }

    private const string homeUri = "http://localhost";
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
        FilterSection = _dom.QuerySelector(FilterContainerDefinition) != null ? new SearchFilterSection(_dom.QuerySelector(FilterContainerDefinition)!) : null;
    }

    public class SearchResults
    {
        private static string SearchResultsNumberDefinition => "#search-results-count";
        private static string SearchResultLinksDefinition => "ul li h4";
        private static string SearchNoResultTextDefinition => "#no-results";

        private IDocument _baseElement;
        public string? ResultsText { get; set; }
        public string? NoResultsText { get; set; }
        public IEnumerable<IElement>? SearchResultLinks { get; set; }

        public SearchResults(IDocument baseElement)
        {
            _baseElement = baseElement;
            ResultsText = _baseElement.QuerySelector(SearchResultsNumberDefinition)?.TextContent;
            SearchResultLinks = _baseElement.GetMultipleElements(HomePage.SearchResultLinks.Criteria);
            NoResultsText = _baseElement.QuerySelector(SearchNoResultTextDefinition)?.TextContent;
        }
    }

    public class SearchForm
    {
        private static string SearchInputDefinition => "#searchKeyWord";
        private static string SearchFormDefinition => "#search-establishments-form";
        private static string SearchButtonDefinition => "#search-establishments button";

        private IDocument _dom;
        private IElement? InputTextBox { get; set; }
        private IHtmlButtonElement? SubmitButton { get; set; }
        private IHtmlButtonElement? ClearButton { get; set; }
        private IHtmlFormElement? Form { get; set; }
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

        public Task SubmitClearAsync()
        {
            string? buttonName = ClearButton?.Name;
            string? buttonValue = ClearButton?.Value;

            if (!string.IsNullOrWhiteSpace(buttonName) &&
                !string.IsNullOrWhiteSpace(buttonValue) &&
                ClearButton != null)
            {
                // simulates the button press by manually adding the new element to the page
                // - TODO - reference an explanation of what's happening or does Aasim do it a better way?
                ClearButton.Name = $"{buttonName}_old";
                IHtmlInputElement input = _dom.CreateElement<IHtmlInputElement>();
                input.Name = buttonName;
                input.Value = buttonValue;
                Form!.AppendChild(input);
            }
            return Form!.SubmitAsync();
        }

        // put this back in for getting  the button properties and submitting based on them (Aasim's way)
        //public async Task<string> SubmitSearch(HttpClient client, Dictionary<string, string> searchCriteria)
        //{
        //    ArgumentNullException.ThrowIfNull(Form);
        //    ArgumentNullException.ThrowIfNull(SubmitButton);

        //    var formResponse = await client.SendAsync(
        //            Form,
        //            SubmitButton,
        //            searchCriteria);
        //    return await formResponse.Content.ReadAsStringAsync();
        //}
    }

    public class SearchFilterSection // the entire container section that contains all the filters
    {
        private static string FiltersHeadingDefinition => "#filters-heading";

        public string? FiltersHeading { get; }
        public IEnumerable<Filter> Filters { get; }

        private IEnumerable<IHtmlFieldSetElement> _filters;
        private IElement _filterContainer;

        public SearchFilterSection(IElement element)
        {
            _filterContainer = element;
            _filters = element.GetNodes<IHtmlFieldSetElement>();
            FiltersHeading = element.QuerySelector(FiltersHeadingDefinition)?.TextContent;

            var filterNames = element.GetElementsByTagName("legend").Select(x => x.InnerHtml.Trim());
            foreach (var filter in _filters)
            {
                var filterName = filter.GetNodes<IHtmlLegendElement>().Single();
                var filterBoxes = filter.GetNodes<IHtmlInputElement>();
            }

            Filters = _filters.Select(filter => new Filter(filter));
        }

        public Filter GetFilterByName(string filterName)
        {
            return Filters.Where(filter => filter.Name == filterName).Single();
        }

        public void SelectFilters(Dictionary<string, string> filters) // key = filter name, value = filter value
        {
            foreach (var filter in filters)
            {
                var filterBoxesForFilterName = GetFilterByName(filter.Key);
                filterBoxesForFilterName.SelectCheckBoxWithValue(filter.Value);
            }
        }
    }

    /// <summary>
    ///  contains everything for one filter option - the Filter name, its possible values and checkboxes
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

        public string Name { get; set; }
        public IEnumerable<string> CheckBoxValues => _checkBoxes.Select(checkbox => checkbox.Value);
        public IEnumerable<string> SelectedCheckBoxValues => _checkBoxes.Where(checkBox => checkBox.IsChecked).Select(checkBox => checkBox.Value);
        public void SelectCheckBoxWithValue(string value)
        {
            _checkBoxes.Where(checkbox => checkbox.Value == value).Single().IsChecked = true;
        }

    }
}