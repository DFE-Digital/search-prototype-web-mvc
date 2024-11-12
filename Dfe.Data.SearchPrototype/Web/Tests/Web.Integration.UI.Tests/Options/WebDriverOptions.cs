namespace DfE.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Options;

public sealed class WebDriverOptions
{
    public const string Key = "webDriver";
    public bool Headless { get; set; }
    public string ScreenshotsDirectory { get; set; } = "screenshots";
    public string? DriverBinaryDirectory { get; set; } = null!;
}

