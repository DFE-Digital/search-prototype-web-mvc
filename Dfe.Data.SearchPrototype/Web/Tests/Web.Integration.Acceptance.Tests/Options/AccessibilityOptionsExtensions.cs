namespace Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Options;

internal static class AccessibilityOptionsExtensions
{
    internal static AccessibilityOptions CreateArtifactOutputDirectory(
        this AccessibilityOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }
        var outputPath = Path.IsPathFullyQualified(options.ReportOutputDirectory) ?
            options.ReportOutputDirectory :
            Path.Combine(Directory.GetCurrentDirectory(), options.ReportOutputDirectory);

        Directory.CreateDirectory(outputPath);
        options.ArtifactsOutputPath = outputPath;
        return options;
    }
}
