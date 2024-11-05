using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using OpenQA.Selenium;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;

public sealed class HomePage : BasePage
{
    public HomePage(IDomQueryClient domQueryClient) : base(domQueryClient)
    {
    }

    public IWebElement HeadingElement => DriverContext.Wait.UntilElementExists(By.CssSelector("header div div:nth-of-type(2) a"));
    public static By Heading => By.CssSelector("#service-name");
    public static By NavigationBarHomeLink => By.CssSelector("#home-link");
    public static By SearchHeading => By.CssSelector("h1 label");
    public static By SearchSubHeading => By.CssSelector("#searchKeyWord-hint");
    public By SearchHiddenDiv => By.CssSelector("#searchKeyWord + div");
    public static By SearchInput => By.CssSelector("#searchKeyWord");
    public static By SearchForm => By.CssSelector("#search-establishments-form");
    public static By SearchButton => By.CssSelector("#search-establishments-form button");
    public static By SearchResultsNumber => By.CssSelector("#search-results-count");
    public static By SearchResultsHeadings => By.CssSelector("ul li h4");
    public static By SearchNoResultText => By.CssSelector("#no-results");
    public static By SearchResultsContainer => By.CssSelector("#results");
    public static By FiltersContainer => By.CssSelector("#filters-container");
    public static By FiltersHeading => By.CssSelector("#filters-heading");
    public static By ApplyFiltersButton => By.CssSelector("#filters-button");
    public static By ClearFiltersButton => By.CssSelector("#clearFilters");
    public static By EstablishmentStatusNameHeading => By.CssSelector("#FacetName-ESTABLISHMENTSTATUSNAME legend");
    public static By OpenFilterInput => By.CssSelector("#ESTABLISHMENTSTATUSNAME-open");
    public static By OpenFilterLabel => By.CssSelector("#ESTABLISHMENTSTATUSNAME-open + label");
    public static By ClosedFilterInput => By.CssSelector("#ESTABLISHMENTSTATUSNAME-closed");
    public static By ClosedFilterLabel => By.CssSelector("#ESTABLISHMENTSTATUSNAME-closed + label");
    public static By ProposedToOpenFilterInput => By.CssSelector("#ESTABLISHMENTSTATUSNAME-proposed-to-open");
    public static By ProposedToOpenFilterLabel => By.CssSelector("#ESTABLISHMENTSTATUSNAME-proposed-to-open + label");
    public static By OpenProposedToCloseFilterInput => By.CssSelector("#ESTABLISHMENTSTATUSNAME-open-but-proposed-to-close");
    public static By OpenProposedToCloseFilterLabel => By.CssSelector("#ESTABLISHMENTSTATUSNAME-open-but-proposed-to-close + label");
    public static By PhaseOfEducationHeading => By.CssSelector("#FacetName-PHASEOFEDUCATION legend");
    public static By PrimaryFilterInput => By.CssSelector("#PHASEOFEDUCATION-primary");
    public static By PrimaryFilterLabel => By.CssSelector("#PHASEOFEDUCATION-primary + label");
    public static By SecondaryFilterInput => By.CssSelector("#PHASEOFEDUCATION-secondary");
    public static By SecondaryFilterLabel => By.CssSelector("#PHASEOFEDUCATION-secondary + label");
    public static By NAFilterInput => By.CssSelector("#PHASEOFEDUCATION-not-applicable");
    public static By NAFilterLabel => By.CssSelector("#PHASEOFEDUCATION-not-applicable + label");
    public static By AllThroughFilterInput => By.CssSelector("#PHASEOFEDUCATION-all-through");
    public static By AllThroughFilterLabel => By.CssSelector("#PHASEOFEDUCATION-all-through + label");
    public static By MiddleDeemedSecondaryFilterInput => By.CssSelector("#PHASEOFEDUCATION-middle-deemed-secondary");
    public static By MiddleDeemedSecondaryFilterLabel => By.CssSelector("#PHASEOFEDUCATION-middle-deemed-secondary + label");
    public static By NurseryFilterInput => By.CssSelector("#PHASEOFEDUCATION-nursery");
    public static By NurseryFilterLabel => By.CssSelector("#PHASEOFEDUCATION-nursery + label");
    public static By SixteenPlusFilterInput => By.CssSelector("#PHASEOFEDUCATION-16-plus");
    public static By SixteenPlusFilterLabel => By.CssSelector("#PHASEOFEDUCATION-16-plus + label");
    public static By MiddleDeemedPrimaryFilterInput => By.CssSelector("#PHASEOFEDUCATION-middle-deemed-primary");
    public static By MiddleDeemedPrimaryFilterLabel => By.CssSelector("#PHASEOFEDUCATION-middle-deemed-primary + label");
    public static By SearchResultEstablishmentStatus => By.CssSelector("[id^=\"establishment-status-\"]");
    public static By SearchResultEstablishmentPhase => By.CssSelector("[id^=\"education-phase-\"]");

    public string? GetHeading() => DomQueryClient.GetText(Heading.Criteria);
    public string? GetNavigationBarHomeText() => DomQueryClient.GetText(NavigationBarHomeLink.Criteria);
    public string? GetSearchHeading() => DomQueryClient.GetText(SearchHeading.Criteria);
    public string? GetSearchSubheading() => DomQueryClient.GetText(SearchSubHeading.Criteria);
    public bool IsSearchInputExists() => DomQueryClient.ElementExists(SearchInput.Criteria);
    public bool IsSearchButtonExists() => DomQueryClient.ElementExists(SearchButton.Criteria);
    public bool IsSearchFormExists() => DomQueryClient.ElementExists(SearchForm.Criteria);
    public string? GetApplyFiltersText() => DomQueryClient.GetText(ApplyFiltersButton.Criteria);
    public string? GetClearFiltersText() => DomQueryClient.GetText("#clearFilters");
    public string? GetFiltersHeading() => DomQueryClient.GetText("#filters-heading");
    public string? GetEstablishmentStatusFiltersHeading() => DomQueryClient.GetText(EstablishmentStatusNameHeading.Criteria);
    
}
