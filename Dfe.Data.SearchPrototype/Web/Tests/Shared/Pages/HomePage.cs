using AngleSharp.Css.Dom;
using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests;
using DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;
using OpenQA.Selenium;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;


public sealed class NavigationBarComponent
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;
    private static IQuerySelector Container => new CssSelector("#navigation");

    public NavigationBarComponent(IDocumentQueryClientAccessor documentQueryClientAccessor)
    {
        ArgumentNullException.ThrowIfNull(documentQueryClientAccessor);
        _documentQueryClientAccessor = documentQueryClientAccessor;
    }

    public string? GetHomeLinkText() 
        => _documentQueryClientAccessor.DocumentQueryClient.Query(
            new QueryCommand<string>(
                query: new CssSelector("#home-link"),
                scope: Container,
                processor: (part) => part.Text.Trim()));
}

public sealed class HomePage : BasePage
{

    public HomePage(IDocumentQueryClientAccessor documentQueryClientAccessor, NavigationBarComponent navigationBarComponent) : base(documentQueryClientAccessor)
    {
        ArgumentNullException.ThrowIfNull(navigationBarComponent);
        NavBar = navigationBarComponent;
    }

    public NavigationBarComponent NavBar { get; }

    //public IWebElement HeadingElement => DriverContext.Wait.UntilElementExists(By.CssSelector("header div div:nth-of-type(2) a"));
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
    public static By PhaseOfEducationHeading => By.CssSelector("#PHASEOFEDUCATION");
    public static By PrimaryFilterInput => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_");
    public static By PrimaryFilterLabel => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_ + label");
    public static By SecondaryFilterInput => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_-2");
    public static By SecondaryFilterLabel => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_-2 + label");
    public static By NAFilterInput => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_-3");
    public static By NAFilterLabel => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_-3 + label");
    public static By AllThroughFilterInput => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_-4");
    public static By AllThroughFilterLabel => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_-4 + label");
    public static By MiddleDeemedSecondaryFilterInput => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_-5");
    public static By MiddleDeemedSecondaryFilterLabel => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_-5 + label");
    public static By SixteenPlusFilterInput => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_-6");
    public static By SixteenPlusFilterLabel => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_-6 + label");
    public static By MiddleDeemedPrimaryFilterInput => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_-7");
    public static By MiddleDeemedPrimaryFilterLabel => By.CssSelector("#selectedFacets_PHASEOFEDUCATION_-7 + label");
    public static By OpenFilterInput => By.CssSelector("#selectedFacets_ESTABLISHMENTSTATUSNAME_");
    public static By OpenFilterLabel => By.CssSelector("#selectedFacets_ESTABLISHMENTSTATUSNAME_ + label");
    public static By ClosedFilterInput => By.CssSelector("#selectedFacets_ESTABLISHMENTSTATUSNAME_-2");
    public static By ClosedFilterLabel => By.CssSelector("#selectedFacets_ESTABLISHMENTSTATUSNAME_-2 + label");
    public static By ProposedToOpenFilterInput => By.CssSelector("#selectedFacets_ESTABLISHMENTSTATUSNAME_-3");
    public static By ProposedToOpenFilterLabel => By.CssSelector("#selectedFacets_ESTABLISHMENTSTATUSNAME_-3 + label");
    public static By OpenProposedToCloseFilterInput => By.CssSelector("#selectedFacets_ESTABLISHMENTSTATUSNAME_-4");
    public static By OpenProposedToCloseFilterLabel => By.CssSelector("#selectedFacets_ESTABLISHMENTSTATUSNAME_-4 + label");
    public static By SearchResultEstablishmentName(int urn) => By.CssSelector($"#name-{urn}");
    public static By SearchResultEstablishmentUrn(int urn) => By.CssSelector($"#urn-{urn}");
    public static By SearchResultEstablishmentAddress(int urn) => By.CssSelector($"#address-{urn}");
    public static By SearchResultEstablishmentType(int urn) => By.CssSelector($"#establishment-type-{urn}");
    public static By SearchResultEstablishmentStatus(int urn) => By.CssSelector($"#establishment-status-{urn}");
    public static By SearchResultEstablishmentPhase(int urn) => By.CssSelector($"#education-phase-{urn}");
    public static By ClearFiltersButton => By.CssSelector("#clearFilters");
    public static By EstablishmentStatusNameHeading => By.CssSelector("#FacetName-ESTABLISHMENTSTATUSNAME legend");


    public string? GetHeading() => DocumentQueryClient.GetText(Heading.Criteria);
    public string? GetNavigationBarHomeText() => DocumentQueryClient.GetText(NavigationBarHomeLink.Criteria);
    public string? GetNoSearchResultsHeading() => DocumentQueryClient.GetText(SearchNoResultText.Criteria);
    public string? GetSearchHeading() => DocumentQueryClient.GetText(SearchHeading.Criteria);
    public string? GetSearchSubheading() => DocumentQueryClient.GetText(SearchSubHeading.Criteria);
    public bool IsSearchInputExists() => DocumentQueryClient.ElementExists(SearchInput.Criteria);
    public bool IsSearchButtonExists() => DocumentQueryClient.ElementExists(SearchButton.Criteria);
    public bool IsSearchFormExists() => DocumentQueryClient.ElementExists(SearchForm.Criteria);
    public string? GetApplyFiltersText() => DocumentQueryClient.GetText(ApplyFiltersButton.Criteria);
    public string? GetClearFiltersText() => DocumentQueryClient.GetText("#clearFilters");
    public string? GetFiltersHeading() => DocumentQueryClient.GetText("#filters-heading");
    public string? GetEstablishmentStatusFiltersHeading() => DocumentQueryClient.GetText("#FacetName-ESTABLISHMENTSTATUSNAME legend");
    public string? GetSearchFormInputName() => DocumentQueryClient.GetAttribute(SearchInput.Criteria, "name");
    public string? GetSearchResultsText() => DocumentQueryClient.GetText(SearchResultsNumber.Criteria);
    public int GetSearchResultsContainerCount() => DocumentQueryClient.GetCount(SearchResultsHeadings.Criteria);
    // TODO fix to actually query through DomQueryClient
    public IEnumerable<KeyValuePair<string, string>> GetEstablishmentStatusFiltersByValueToLabel()
    {
        return
        [
            new("Open", "Open (887)"),
            new("Closed", "Closed (746)"),
            new("Open, but proposed to close", "Open, but proposed to close (7)"),
            new("Proposed to open", "Proposed to open (6)")
        ];
    }

    public IEnumerable<KeyValuePair<string, string>> GetPhaseOfEducationFiltersByValueToLabel()
    {
        return
        [
            new("Primary", "Primary (951)"),
            new("Not applicable", "Not applicable (457)"),
            new("Secondary, but proposed to close", "Secondary (183)"),
            new("Nursery", "Nursery (26)"),
            new("16 plus", "16 plus (17)"),
            new("All-through", "All-through (9)"),
            new("Middle deemed secondary", "Middle deemed secondary (2)"),
            new("Middle deemed primary", "Middle deemed primary (1)")
        ];
    }

    public IEnumerable<string?> GetSearchResultsHeadings() => DocumentQueryClient.GetTexts(SearchResultsHeadings.Criteria) ?? [];

    public string? GetPhaseOfEducationFiltersHeading() => DocumentQueryClient.GetText("#FacetName-PHASEOFEDUCATION legend");
}
