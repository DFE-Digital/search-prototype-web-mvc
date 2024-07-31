﻿using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;
using OpenQA.Selenium;

namespace Dfe.Data.SearchPrototype.Web.Tests.PageObjectModel;

public sealed class SearchPage : BasePage
{
    public SearchPage(IWebDriverContext driverContext) : base(driverContext)
    {
    }

    public IWebElement HeadingElement => DriverContext.Wait.UntilElementExists(By.CssSelector("header div div:nth-of-type(2) a"));
    public static By Heading => By.CssSelector("header div div:nth-of-type(2) a");
    public static By HomeLink => By.CssSelector("nav a");
    public static By SearchHeading => By.CssSelector("h1 label");
    public static By SearchSubHeading => By.CssSelector("#searchKeyWord-hint");
    public By SearchHiddenDiv => By.CssSelector("#searchKeyWord + div");
    public static By SearchInput => By.CssSelector("#searchKeyWord");
    public static By SearchForm => By.CssSelector("#main-content form");
    public static By SearchButton => By.CssSelector("#main-content form button");
    public static By SearchResultsNumber => By.CssSelector(".govuk-heading-m");
    public static By SearchResultLinks => By.CssSelector("ul li h4 a");
    public static By SearchNoResultText => By.CssSelector("#main-content form + p");
}
