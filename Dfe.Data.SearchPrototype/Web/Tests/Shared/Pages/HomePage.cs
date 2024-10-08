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
    public static By SearchResultLinks => By.CssSelector("ul li h4");
    public static By SearchNoResultText => By.CssSelector("#no-results");
    public static By SearchResultsContainer => By.CssSelector("#results");
    public static By FiltersHeading => By.CssSelector("#filters-heading");
    public static By SearchResultEstablishmentName(int urn) => By.CssSelector($"#name-{urn}");
    public static By SearchResultEstablishmentUrn(int urn) => By.CssSelector($"#urn-{urn}");
    public static By SearchResultEstablishmentAddress(int urn) => By.CssSelector($"#address-{urn}");
    public static By SearchResultEstablishmentType(int urn) => By.CssSelector($"#establishment-type-{urn}");
    public static By SearchResultEstablishmentStatus(int urn) => By.CssSelector($"#establishment-status-{urn}");
    public static By SearchResultEstablishmentPhase(int urn) => By.CssSelector($"#education-phase-{urn}");

}
