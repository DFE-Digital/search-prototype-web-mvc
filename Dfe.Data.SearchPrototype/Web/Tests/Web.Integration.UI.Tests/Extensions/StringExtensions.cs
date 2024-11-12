using System.Text.RegularExpressions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Extensions;

public static class StringExtensions
{
    public static readonly Regex HtmlReplacer = new("<[^>]*>");
    public static string? ToLowerRemoveHyphens(this string? str)
        => string.IsNullOrEmpty(str) ? str : str.Replace(' ', '-').ToLower();

    public static string? ReplaceHTML(this string? str)
        => string.IsNullOrEmpty(str) ? str : HtmlReplacer.Replace(str, string.Empty);

    public static string SanitiseToHTML(string input)
    {
        var ouput = input.Replace("\"", "\'");
        return ouput;
    }
}
