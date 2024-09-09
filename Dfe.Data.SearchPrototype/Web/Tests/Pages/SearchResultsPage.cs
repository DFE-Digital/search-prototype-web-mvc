using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;
using Dfe.Data.SearchPrototype.Web.Tests.PageObjectModel;
using OpenQA.Selenium;

namespace Dfe.Data.SearchPrototype.Web.Tests.Pages.Search;

public sealed class SearchResultsPage : BasePage
{
    public SearchResultsPage(IWebDriverContext driverContext) : base(driverContext) { }

    //TODO: Add ids when they are implemented into web

    public static By SearchResultsNumber => By.CssSelector("#search-results-count");
    public static By SearchResultLinks => By.CssSelector("ul li h4 a");
    public static By SearchNoResultText => By.CssSelector("#no-results");
    public static By SearchResultEstablishmentName(int urn) => By.CssSelector($"#name-{urn}");
    public static By SearchResultEstablishmentUrn(int urn) => By.CssSelector($"#urn-{urn}");
    public static By SearchResultEstablishmentAddress(int urn) => By.CssSelector($"#address-{urn}");
    public static By SearchResultEstablishmentType(int urn) => By.CssSelector($"#establishment-type-{urn}");
    public static By SearchResultEstablishmentStatus(int urn) => By.CssSelector($"#establishment-status-{urn}");
    public static By SearchResultEstablishmentPhase(int urn) => By.CssSelector($"#education-phase-{urn}");

}