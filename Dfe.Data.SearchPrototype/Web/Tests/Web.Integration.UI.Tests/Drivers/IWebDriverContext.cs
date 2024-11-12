using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Drivers;

public interface IWebDriverContext : IDisposable
{
    IWebDriver Driver { get; }
    IWait<IWebDriver> Wait { get; }
    void GoToUri(string path);
    void GoToUri(string baseUri, string path);
    void TakeScreenshot(ITestOutputHelper logger, string testName);
}

