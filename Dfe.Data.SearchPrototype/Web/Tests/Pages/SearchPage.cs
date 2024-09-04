using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;
using OpenQA.Selenium;

namespace Dfe.Data.SearchPrototype.Web.Tests.PageObjectModel;

public sealed class SearchPage : BasePage
{
    public SearchPage(IWebDriverContext driverContext) : base(driverContext)
    {
    }

    public IWebElement HeadingElement => DriverContext.Wait.UntilElementExists(By.CssSelector("#service-name"));
    public static By Heading => By.CssSelector("#service-name");
    public static By HomeLink => By.CssSelector("#home-link");
    public static By SearchHeading => By.CssSelector("#page-heading");
    public static By SearchSubHeading => By.CssSelector("#searchKeyWord-hint");
    public By SearchHiddenDiv => By.CssSelector("#searchKeyWord + div");
    public static By SearchInput => By.CssSelector("#searchKeyWord");
    public static By SearchForm => By.CssSelector("#search-establishments");
    public static By SearchButton => By.CssSelector("#search-establishments button");

}
