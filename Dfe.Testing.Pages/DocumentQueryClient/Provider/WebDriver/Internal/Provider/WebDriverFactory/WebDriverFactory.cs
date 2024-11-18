using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.SessionOptions;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.Provider.WebDriverFactory;

// TODO WHEN REMOTEDRIVER GRID - DriverService is only used for local driver executeable management, over a remote implementation not available build with variance in mind ... 
internal abstract class WebDriverFactoryBase<TDriver>
    where TDriver : IWebDriver
{
    public abstract Task<Func<TDriver>> CreateDriver(WebDriverSessionOptions sessionOptions);
}
