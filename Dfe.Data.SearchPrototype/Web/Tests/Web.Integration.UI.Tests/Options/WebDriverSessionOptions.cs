namespace DfE.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Options;

public sealed class WebDriverSessionOptions
{
    public string Browser { get; set; } = "chrome";
    public string Device { get; set; } = "desktop";
    public bool DisableJs { get; set; } = false;
}
