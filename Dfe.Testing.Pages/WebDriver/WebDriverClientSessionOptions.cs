namespace Dfe.Testing.Pages.WebDriver;

public sealed class WebDriverClientSessionOptions
{
    public string BrowserName { get; set; } = string.Empty;
    public int PageLoadTimeout { get; set; } = 45;
    public int RequestTimeout { get; set; } = 60;
    public bool EnableNetworkInterception { get; set; } = false;
    public bool EnableVerboseLogging { get; set; } = false;
}
