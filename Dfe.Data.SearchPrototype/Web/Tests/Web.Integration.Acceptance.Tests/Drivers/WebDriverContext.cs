using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Drawing;
using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Options;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;

public class WebDriverContext : IWebDriverContext
{
    private readonly WebDriverOptions _driverOptions;
    // TODO: reimplement Lazy<IWebDriver>
    private readonly IWebDriver _driver;
    private readonly Lazy<IWait<IWebDriver>> _wait;
    private readonly string _baseUri;

    public IWebDriver Driver => _driver;
    public IWait<IWebDriver> Wait => _wait.Value;
    private Type[] IgnoredExceptions { get; } = [typeof(StaleElementReferenceException)];

    public WebDriverContext(
        IWebDriverFactory factory,
        IOptions<WebOptions> options,
        IOptions<WebDriverOptions> driverOptions
    )
    {
        _driver = factory?.CreateDriver() ?? throw new ArgumentNullException(nameof(factory));
        _baseUri = options.Value.GetWebUri() ?? throw new ArgumentNullException(nameof(options));
        _wait = new(() => InitializeWait(Driver, IgnoredExceptions));
        _driverOptions = driverOptions.Value;
    }

    private IJavaScriptExecutor JsExecutor =>
        Driver as IJavaScriptExecutor ??
        throw new ArgumentNullException(nameof(IJavaScriptExecutor));

    /// <summary>
    /// Navigate to relative path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public void GoToUri(string path) => GoToUri(_baseUri, path);

    /// <summary>
    /// Navigate to a uri
    /// </summary>
    /// <param name="baseUri">baseUri for site e.g https://google.co.uk</param>
    /// <param name="path">path from baseUri defaults to '/' e.g '/login'</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void GoToUri(string baseUri, string path = "/")
    {
        _ = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
        var absoluteUri = $"{baseUri.TrimEnd('/')}{path}";
        if (Uri.TryCreate(absoluteUri, UriKind.Absolute, out var uri))
        {
            Driver.Navigate().GoToUrl(uri);
        }
        else
        {
            throw new ArgumentException(nameof(absoluteUri));
        }
    }

    /// <summary>
    /// Dispose of <see cref="IWebDriver"/>
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            using (Driver)
            {
                Driver.Quit();
            }
        }
    }

    public void TakeScreenshot(ITestOutputHelper logger, string testName)
    {

        // Allows alternative path
        var baseScreenshotDirectory =
            Path.IsPathFullyQualified(_driverOptions.ScreenshotsDirectory) ?
                _driverOptions.ScreenshotsDirectory :
                Path.Combine(Directory.GetCurrentDirectory(), _driverOptions.ScreenshotsDirectory);

        Directory.CreateDirectory(baseScreenshotDirectory);

        var outputPath = Path.Combine(
            baseScreenshotDirectory,
            testName + ".png"
        );

        // Maximise viewport
        Driver.Manage().Window.Size = new Size(
            width: GetBrowserWidth(),
            height: GetBrowserHeight()
        );

        // Screenshot
        (Driver as ITakesScreenshot)?
            .GetScreenshot()
            .SaveAsFile(outputPath);

        logger.WriteLine($"SCREENSHOT SAVED IN LOCATION: {outputPath}");
    }

    private static IWait<IWebDriver> InitializeWait(IWebDriver driver, Type[] ignoredExceptions)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        wait.IgnoreExceptionTypes(ignoredExceptions);
        return wait;
    }

    private int GetBrowserWidth() =>
    // Math.max returns a 64 bit number requiring casting
        (int)(long)JsExecutor.ExecuteScript(
        @"return Math.max(
                    window.innerWidth,
                    document.body.scrollWidth,
                    document.documentElement.scrollWidth)"
        );

    private int GetBrowserHeight() =>
        // Math.max returns a 64 bit number requiring casting
        (int)(long)JsExecutor.ExecuteScript(
            @"return Math.max(
                    window.innerHeight,
                    document.body.scrollHeight,
                    document.documentElement.scrollHeight)"
        );
}