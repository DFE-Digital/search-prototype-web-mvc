using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Extensions;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;

internal class AngleSharpQueryClient : IDomQueryClient
{
    private readonly IHtmlDocument _htmlDocument;
    public AngleSharpQueryClient(IHtmlDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        _htmlDocument = document;
    }

    public virtual string GetText(string cssSelector)
        => Find(cssSelector).TextContent.Trim();

    public virtual IEnumerable<string> GetTexts(string cssSelector)
        => FindAll(cssSelector)
            .Select(t => t.TextContent.Trim());

    public virtual string? GetAttribute(string cssSelector, string attribute)
    {
        ArgumentException.ThrowIfNullOrEmpty(attribute);
        return Find(cssSelector).GetAttribute(attribute);
    }
    public virtual string? GetLink(string cssSelector) => GetAttribute(cssSelector, "href");
    public bool ElementExists(string cssSelector) => (TryFindOrDefault(cssSelector) != null);

    private IElement Find(string cssSelector)
        => FindAll(cssSelector)
            .ThrowIfMultiple()
            .Single();

    private IEnumerable<IElement> FindAll(string cssSelector) => TryFindOrDefault(cssSelector)! ?? [];
    
    private IEnumerable<IElement>? TryFindOrDefault(string cssSelector)
    {
        ArgumentException.ThrowIfNullOrEmpty(cssSelector);
        return _htmlDocument.QuerySelectorAll(cssSelector);
    }
}
