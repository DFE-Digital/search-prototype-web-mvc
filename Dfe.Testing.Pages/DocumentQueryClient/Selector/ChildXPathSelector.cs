namespace Dfe.Testing.Pages.DocumentQueryClient.Selector;

public sealed class ChildXPathSelector : IElementSelector
{
    private const string childrenXpathPrefix = ".//";
    private readonly string _xpath;

    public ChildXPathSelector(string? selector = null)
    {
        _xpath = string.IsNullOrEmpty(selector) ? "*"  : selector;
    }

    public string ToSelector()
        => _xpath.StartsWith(childrenXpathPrefix) ? 
            _xpath : 
            $"{childrenXpathPrefix}{_xpath}";
    
}
