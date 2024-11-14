namespace Dfe.Testing.Pages.DocumentQueryClient.Selector;

public sealed class IdSelector : IElementSelector
{
    private readonly string _selector;

    public IdSelector(string selector)
    {
        ArgumentNullException.ThrowIfNull(_selector, nameof(selector));
        _selector = selector;
    }
    public string ToSelector() => _selector.StartsWith("#") ? _selector : $"#{_selector}";
    
}
