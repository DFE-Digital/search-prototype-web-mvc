using AngleSharp;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;

public class TinySearchPageModel : ISearchPage
{
    private AngleSharpTinySearchPageBuilder _builder;

    public TinySearchPageModel(IBrowsingContext _browsingContext)
    {
        _builder = new AngleSharpTinySearchPageBuilder(_browsingContext);
    }

    public string? PageHeading { get => _builder.MainHeading(); }
    public string? NoResultsText { get => _builder.NoResultsText(); }
    public string? ResultsText { get => _builder.ResultsText(); }
    public string? FilterSectionHeading { get => _builder.FilterSectionHeading(); }
    public IEnumerable<KeyValuePair<string, string>>? Filters { get => _builder.Filters(); }
    public IEnumerable<KeyValuePair<string, string>>? SelectedFilters { get => _builder.SelectedFilters(); }

    public bool FilterSectionIsNullOrEmpty { get => _builder.FilterSectionIsNullOrEmpty(); }

    public Task NavigateToPage(string url) => _builder.NavigateToPage(url);
    public void SelectFilters(Dictionary<string, string> filters) => _builder.SelectFilters(filters);
    public Task SubmitAsync() => _builder.SubmitAsync();
    public Task SubmitClearAsync() => _builder.SubmitClearAsync();
}

