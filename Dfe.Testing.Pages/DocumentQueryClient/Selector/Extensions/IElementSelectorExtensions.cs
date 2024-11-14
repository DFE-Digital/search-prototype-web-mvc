namespace Dfe.Testing.Pages.DocumentQueryClient.Selector.Extensions;

internal static class IElementSelectorExtensions
{
    internal static bool IsSelectorXPathConvention(this IElementSelector selector)
    {
        const string entireDocumentPrefix = "//";
        var selectWith = selector.ToSelector();
        return (selectWith.StartsWith(entireDocumentPrefix) || selectWith.StartsWith(".//"));
    }
}
