using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;
using OpenQA.Selenium;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;

public sealed class HomePage : BasePage
{
    public HomePage(IWebDriverContext driverContext) : base(driverContext)
    {
    }

    public IWebElement HeadingElement => DriverContext.Wait.UntilElementExists(By.CssSelector("header div div:nth-of-type(2) a"));
    public static By Heading => By.CssSelector("header div div:nth-of-type(2) a");
    public static By HomeLink => By.CssSelector("nav a");
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
    public static By PhaseOfEducationHeading => By.CssSelector("#filters-container div:nth-child(3) fieldset legend");
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
    public static By EstablishmentStatusNameHeading => By.CssSelector("#filters-container div:nth-child(4) fieldset legend");
    public static By SearchResultEstablishmentName(int urn) => By.CssSelector($"#name-{urn}");
    public static By SearchResultEstablishmentUrn(int urn) => By.CssSelector($"#urn-{urn}");
    public static By SearchResultEstablishmentAddress(int urn) => By.CssSelector($"#address-{urn}");
    public static By SearchResultEstablishmentType(int urn) => By.CssSelector($"#establishment-type-{urn}");
    public static By SearchResultEstablishmentStatus(int urn) => By.CssSelector($"#establishment-status-{urn}");
    public static By SearchResultEstablishmentPhase(int urn) => By.CssSelector($"#education-phase-{urn}");

}
