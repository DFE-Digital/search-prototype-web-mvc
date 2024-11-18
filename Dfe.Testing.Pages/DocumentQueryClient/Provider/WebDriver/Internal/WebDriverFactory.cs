using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal;

// TODO WHEN REMOTEDRIVER GRID - DriverService is only used for local driver executeable management, over a remote implementation not available build with variance in mind ... 
internal abstract class WebDriverFactoryBase<TDriver>
    where TDriver : IWebDriver
{
    public abstract Task<Func<TDriver>> CreateDriver(WebDriverSessionOptions sessionOptions);
}

internal sealed class ChromeDriverFactory : WebDriverFactoryBase<ChromeDriver>
{
    public override Task<Func<ChromeDriver>> CreateDriver(WebDriverSessionOptions sessionOptions)
    {
        // TODO could enable caching driverService in the abstract class - so that when new ChromeDriver it's created from the cached instance?
        var driverService = ChromeDriverService.CreateDefaultService();
        return Task.FromResult(() => new ChromeDriver(driverService));
    }
}
