using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using Microsoft.Extensions.Options;
using System.Drawing;
using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Options;
 
namespace Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;
 
public sealed class WebDriverFactory : IWebDriverFactory
{
    private static readonly IEnumerable<string> DEFAULT_OPTIONS = new[]
    {
            "--incognito",
            "--safebrowsing-disable-download-protection",
            "--no-sandbox",
            "--start-maximized",
            "--start-fullscreen"
    };
 
    private static readonly Dictionary<string, (int x, int y)> MOBILE_VIEWPORTS = new()
    {
        { "desktop", (1920, 1080) },
        { "iphone14", (390, 844) },
        { "iphone11", (414, 896) }
    };
 
    private static TimeSpan DEFAULT_PAGE_LOAD_TIMEOUT = TimeSpan.FromSeconds(30);

    private readonly WebDriverOptions _webDriverOptions;

    private readonly WebDriverSessionOptions _sessionOptions;
 
    public WebDriverFactory(
        IOptions<WebDriverOptions> webDriverOptions,
        WebDriverSessionOptions sessionOptions
    )
    {
        _webDriverOptions = webDriverOptions?.Value ?? throw new ArgumentNullException(nameof(webDriverOptions));
        _sessionOptions = sessionOptions ?? throw new ArgumentNullException(nameof(_sessionOptions));
    }
 
    public IWebDriver CreateDriver()
    {
        // viewports are expressed as cartesian coordinates (x,y)
        var viewportDoesNotExist = !MOBILE_VIEWPORTS.TryGetValue(_sessionOptions.Device, out var viewport);

        if (viewportDoesNotExist)
        {
            throw new ArgumentException($"device value {_sessionOptions.Device} has no mapped viewport");
        }
        var (width, height) = viewport;

        _webDriverOptions.DriverBinaryDirectory ??= Directory.GetCurrentDirectory();

        IWebDriver driver = _sessionOptions switch
        {
            { DisableJs: true } or { Browser: "firefox" } => CreateFirefoxDriver(_webDriverOptions, _sessionOptions),
            _ => CreateChromeDriver(_webDriverOptions)
        };

        driver.Manage().Window.Size = new Size(width, height);
        driver.Manage().Cookies.DeleteAllCookies();
        driver.Manage().Timeouts().PageLoad = DEFAULT_PAGE_LOAD_TIMEOUT;
        return driver;
 
    }
 
    private static ChromeDriver CreateChromeDriver(
        WebDriverOptions driverOptions
    )
    {
        ChromeOptions option = new();

        option.AddArguments(DEFAULT_OPTIONS);
 
        // chromium based browsers using new headless switch https://www.selenium.dev/blog/2023/headless-is-going-away/

        if (driverOptions.Headless)
        {
            option.AddArgument("--headless=new");
        }

        option.AddUserProfilePreference("safebrowsing.enabled", true);
        option.AddUserProfilePreference("download.prompt_for_download", false);
        option.AddUserProfilePreference("disable-popup-blocking", "true");
        option.AddArgument("--window-size=1920,1080");
        return new ChromeDriver(driverOptions.DriverBinaryDirectory, option);
    }
 
    private static FirefoxDriver CreateFirefoxDriver(
        WebDriverOptions driverOptions,
        WebDriverSessionOptions sessionOptions
    )
    {
        var options = new FirefoxOptions
        {
            // TODO load TLS cert into firefox options
            AcceptInsecureCertificates = true,
            EnableDevToolsProtocol = true,
        };
 
        options.AddArguments(DEFAULT_OPTIONS);
 
        if (driverOptions.Headless)
        {
            options.AddArgument("--headless");
        }

        if (sessionOptions.DisableJs)
        {
            options.SetPreference("javascript.enabled", false);
        }

        return new(driverOptions.DriverBinaryDirectory, options);
    }
}