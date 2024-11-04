namespace Dfe.Data.SearchPrototype.Web.Views.Shared;

public static class StringUtils
{
    public static string? FormatToHTMLId(this string str)
    => string.IsNullOrEmpty(str) ? null :
        str.ToLowerInvariant()
        .Replace(' ', '-')
        .Replace('/', '-')
        .Replace(",", string.Empty)
        .Replace("(", string.Empty)
        .Replace(")", string.Empty)
        .Replace("+", string.Empty)
        .Replace("\"", string.Empty)
        .Replace("\'", string.Empty)
        .Replace("%", "percentage")
        .Replace("&", "and");
}