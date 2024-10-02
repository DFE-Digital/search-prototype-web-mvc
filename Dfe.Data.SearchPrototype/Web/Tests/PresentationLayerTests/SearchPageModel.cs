using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;

namespace Dfe.Data.SearchPrototype.Web.Tests.PresentationLayerTests;

public class SearchPageModel
{
    private IDocument _dom;
    public SearchResults Results { get; set; }
    public SearchForm Form { get; set; }
    public FilterSection Filters { get; set; }

    public SearchPageModel(IDocument dom)
    {
        _dom = dom;
        Results = new SearchResults(dom);
        Form = new SearchForm(dom);
        Filters = new FilterSection(dom);
    }

    public class SearchResults
    {
        private static string SearchResultsContainerDefinition => "#results";
        private static string SearchResultsNumberDefinition => "#search-results-count";
        private static string SearchResultLinksDefinition => "ul li h4";
        private static string SearchNoResultTextDefinition => "#no-results";

        private IElement? baseElement;
        public string? ResultsText { get; set; }
        public IEnumerable<IElement>? SearchResultLinks { get; set; }
        public string? NoResultsText { get; set; }

        public SearchResults(IDocument dom)
        {
            baseElement = dom.QuerySelector(SearchResultsContainerDefinition);
            ResultsText = dom.QuerySelector(SearchResultsNumberDefinition)?.TextContent;
            SearchResultLinks = baseElement?.GetMultipleElements(HomePage.SearchResultLinks.Criteria);
            NoResultsText = dom.QuerySelector(SearchNoResultTextDefinition)?.TextContent;
        }
    }

    public class SearchForm
    {
        private static string SearchInputDefinition => "#searchKeyWord";
        private static string SearchFormDefinition => "#search-establishments";
        private static string SearchButtonDefinition => "#search-establishments button";

        public IElement? InputTextBox { get; set; }
        public IHtmlButtonElement? SubmitButton { get; set; }
        public IHtmlFormElement? Form { get; set; }
        public SearchForm(IDocument dom)
        {
            InputTextBox = dom.QuerySelector(SearchInputDefinition);
            SubmitButton = dom.QuerySelector<IHtmlButtonElement>(SearchButtonDefinition);
            Form = dom.QuerySelector<IHtmlFormElement>(SearchFormDefinition);
        }

        public async Task<string> SubmitSearch(HttpClient client, Dictionary<string, string> searchCriteria)
        {
            ArgumentNullException.ThrowIfNull(Form);
            ArgumentNullException.ThrowIfNull(SubmitButton);

            var formResponse = await client.SendAsync(
                    Form,
                    SubmitButton,
                    searchCriteria);
            return await formResponse.Content.ReadAsStringAsync();
        }
    }

    public class FilterSection
    {
        private static string FiltersHeadingDefinition => "#filters-heading";

        public string? FiltersHeading { get; set; }
        public IEnumerable<string>? FacetNames { get; set; }
        public IEnumerable<IHtmlInputElement>? FilterBoxes { get; set; }
        public FilterSection(IDocument dom)
        {
            FiltersHeading = dom.QuerySelector(FiltersHeadingDefinition)?.TextContent;
            FacetNames = dom.GetElementsByTagName("legend").Select(x => x.InnerHtml.Trim());
            FilterBoxes = dom.All
                .Where(element => element.Id != null && element.Id.Contains("selectedFacets_"))
                .Select(e => e as IHtmlInputElement);
        }

        public IEnumerable<IHtmlInputElement>? FilterBoxesForFacet(string facetName)
        {
            return FilterBoxes?.Where(element => element.Id != null && element.Id.Contains($"selectedFacets_{facetName}"));
        }
    }
}
