using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;
using Dfe.Data.SearchPrototype.Web.Tests.PageObjectModel;
using OpenQA.Selenium;

namespace Dfe.Data.SearchPrototype.Web.Tests.Pages.Search;

public sealed class SearchResultsPage : BasePage
{
    public SearchResultsPage(IWebDriverContext driverContext) : base(driverContext) { }

    //TODO: Add ids when they are implemented into web

    public static By SearchResultsNumber => By.CssSelector(".govuk-heading-m");
    public static By SearchResultLinks => By.CssSelector("ul li h4 a");
    public static By SearchNoResultText => By.CssSelector("#main-content form + p");
    public static By FirstResultEstablishmentName => By.CssSelector("ul li h4 a");
    public static By FirstResultEstablishmentUrn => By.CssSelector("ul li:nth-of-type(2)");
    public static By FirstResultEstablishmentAddress => By.CssSelector("ul li:nth-of-type(3)");
    public static By FirstResultEstablishmentType => By.CssSelector("ul li:nth-of-type(4)");
    public static By FirstResultEstablishmentStatus => By.CssSelector("ul li:nth-of-type(5)");
    public static By FirstResultEstablishmentPhase => By.CssSelector("ul li:nth-of-type(6)");
    public static By SecondResultEstablishmentName => By.CssSelector("ul li:nth-of-type(7)");
    public static By SecondResultEstablishmentUrn => By.CssSelector("ul li:nth-of-type(8)");
    public static By SecondResultEstablishmentAddress => By.CssSelector("ul li:nth-of-type(9)");
    public static By SecondResultEstablishmentType => By.CssSelector("ul li:nth-of-type(10)");
    public static By SecondResultEstablishmentStatus => By.CssSelector("ul li:nth-of-type(11)");
    public static By SecondResultEstablishmentPhase => By.CssSelector("ul li:nth-of-type(12)");

}
