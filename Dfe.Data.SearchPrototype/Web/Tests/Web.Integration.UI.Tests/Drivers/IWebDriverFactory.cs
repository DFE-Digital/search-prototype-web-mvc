using OpenQA.Selenium;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Drivers;

public interface IWebDriverFactory
{
    // TODO: reimplement Lazy<IWebDriver>
    IWebDriver CreateDriver();
}
