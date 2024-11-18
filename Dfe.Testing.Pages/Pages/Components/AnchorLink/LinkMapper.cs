namespace Dfe.Testing.Pages.Pages.Components.AnchorLink;
public sealed class LinkMapper
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;

    public LinkMapper(IDocumentQueryClientAccessor documentQueryClientAccessor)
    {
        _documentQueryClientAccessor = documentQueryClientAccessor;
    }

    private static Func<IDocumentPart, Link> MapToLink =>
        (documentPart)
            => new(
                LinkValue: documentPart.GetAttribute("href")!,
                Text: documentPart.Text.Trim(),
                OpensInNewTab: documentPart.GetAttribute("target") == "_blank");

    public Link GetLink(IElementSelector selector, IElementSelector? scope = null)
        => _documentQueryClientAccessor.DocumentQueryClient.Query(
            args: new QueryArgs(selector, scope), 
            mapper: MapToLink);

    public IEnumerable<Link> GetMultipleLinks(IElementSelector selector, IElementSelector? scope = null)
        => _documentQueryClientAccessor.DocumentQueryClient.QueryMany(
            args: new QueryArgs(selector, scope),
            mapper: MapToLink);
}