namespace Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Options;

public sealed class AccessibilityOptions
{
    public const string Key = "accessibility";
    public string ArtifactsOutputPath { get; set; } = string.Empty;
    public string ReportOutputDirectory { get; set; } = "axe-reports";
    public string[] WcagTags { get; set; } = new[] { "wcag2a", "wcag2aa", "wcag21a", "wcag21aa" };
}

