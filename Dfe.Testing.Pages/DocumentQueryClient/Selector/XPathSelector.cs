namespace Dfe.Testing.Pages.DocumentQueryClient.Selector;

public sealed class XPathSelector : IElementSelector
{
    private const string xPathPrefix = "//";
    private readonly string _xpath;

    public XPathSelector(string selector)
    {
        _xpath = string.IsNullOrEmpty(selector) ? string.Empty : selector;
    }

    public string ToSelector()
        => _xpath.StartsWith(xPathPrefix) ?
            _xpath :
            $"{xPathPrefix}{_xpath}";
}
