namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal;
public enum BrowserType
{
    Chrome,
    Firefox,
    Edge
}
internal static class BrowserTypeExtensions
{
    internal static string ToBrowserName(this BrowserType browserType)
        => browserType switch
        {
            BrowserType.Chrome => "chrome",
            BrowserType.Firefox => "firefox",
            BrowserType.Edge => "edge",
            _ => throw new NotImplementedException($"unsupported browser type {browserType}")
        };
}