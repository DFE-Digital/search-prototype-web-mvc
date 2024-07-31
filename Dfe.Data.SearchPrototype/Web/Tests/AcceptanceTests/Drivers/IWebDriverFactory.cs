using OpenQA.Selenium;

namespace Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;

public interface IWebDriverFactory
{
    // TODO: reimplement Lazy<IWebDriver>
    IWebDriver CreateDriver();
}
