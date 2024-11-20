using Dfe.Testing.Pages.WebDriver.Internal;

namespace Dfe.Testing.Pages.WebDriver.Internal.SessionOptions;
internal sealed class WebDriverSessionOptions
{
    public BrowserType BrowserType { get; set; }
    public TimeSpan PageLoadTimeout { get; set; }
    public TimeSpan RequestTimeout { get; set; }
    public bool IsNetworkInterceptionEnabled { get; set; }
    // TODO should the options be a list or dict<list> mapping? { chrome: { ... }, { edge: { ... }, {default: {...}
    public IDictionary<BrowserType, IEnumerable<string>> BrowserOptions { get; set; } = new Dictionary<BrowserType, IEnumerable<string>>();
}