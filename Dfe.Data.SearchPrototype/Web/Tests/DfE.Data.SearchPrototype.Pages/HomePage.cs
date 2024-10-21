using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace DfE.Data.SearchPrototype.Pages;

public sealed class HomePage : BasePage
{
    public HomePage(IDomQueryClient domQueryClient) : base(domQueryClient)
    {

    }

    // TODO improve locator?
    public string GetHeading() => DomQueryClient.GetText("header div div:nth-of-type(2) a");
    public string GetNoSearchCriteriaText() => DomQueryClient.GetText("");
    public bool IsFiltersExists() => DomQueryClient.ElementExists("#filters-container");
    public bool IsSearchResultsNumberExists() => DomQueryClient.ElementExists("#search-results-count");
}

public abstract class BasePage
{

    protected BasePage(IDomQueryClient domQueryClient)
    {
        ArgumentNullException.ThrowIfNull(domQueryClient);
        DomQueryClient = domQueryClient;
    }

    protected IDomQueryClient DomQueryClient { get; }
}

public class HomePageAssertions : ReferenceTypeAssertions<HomePage, HomePageAssertions>
{
    public HomePageAssertions(HomePage instance)
        : base(instance)
    {
    }

    protected override string Identifier => "homePage";


    [CustomAssertion]
    public AndConstraint<HomePageAssertions> HaveNoSearchResults(string because = "", params object[] becauseArgs)
    {
        const string expectedNoSearchResultsHeading = "Sorry no results found please amend your search criteria";
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(
                Subject.GetNoSearchCriteriaText().Contains(expectedNoSearchResultsHeading, StringComparison.Ordinal))
            .FailWith($"Heading should have text: {expectedNoSearchResultsHeading}")
            .Then
            .ForCondition(
                Subject.IsFiltersExists())
            .FailWith($"Filters should not exist")
            .Then
            .ForCondition(
                Subject.IsSearchResultsNumberExists())
            .FailWith("Search results numbering should not exist");

        return new(this);
    }
}

public static class HomePageAssertionExtensions
{
    public static HomePageAssertions Should(this HomePage instance) => new HomePageAssertions(instance);
}