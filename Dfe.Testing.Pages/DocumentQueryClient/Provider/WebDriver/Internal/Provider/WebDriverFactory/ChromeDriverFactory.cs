using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.SessionOptions;
using OpenQA.Selenium.Chrome;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.Provider.WebDriverFactory;
internal sealed class ChromeDriverFactory : WebDriverFactoryBase<ChromeDriver>
{
    public override Task<Func<ChromeDriver>> CreateDriver(WebDriverSessionOptions sessionOptions)
    {
        // TODO could enable caching driverService in the abstract class - so that when new ChromeDriver it's created from the cached instance?
        var driverService = ChromeDriverService.CreateDefaultService();
        return Task.FromResult(
            () => new ChromeDriver(driverService));
    }
}