namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.AnchorLink;
public sealed class LinkComponent
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;

    public LinkComponent(IDocumentQueryClientAccessor documentQueryClientAccessor)
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
            args: new QueryRequest(selector, scope),
            mapper: MapToLink);

    public IEnumerable<Link> GetMultipleLinks(IElementSelector selector, IElementSelector? scope = null)
        => _documentQueryClientAccessor.DocumentQueryClient.QueryMany(
            args: new QueryRequest(selector, scope),
            mapper: MapToLink);
}