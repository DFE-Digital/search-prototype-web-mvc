using OpenQA.Selenium;

namespace DfE.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Drivers;

public interface IWebDriverFactory
{
    // TODO: reimplement Lazy<IWebDriver>
    IWebDriver CreateDriver();
}
