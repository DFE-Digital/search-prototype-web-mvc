namespace Dfe.Testing.Pages.Pages.Components.AnchorLink;
public sealed class LinkQueryCommand
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;

    public LinkQueryCommand(IDocumentQueryClientAccessor documentQueryClientAccessor)
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
    {
        QueryCommand<Link> queryCommand = new(selector, MapToLink, scope);
        return _documentQueryClientAccessor.DocumentQueryClient.Query(queryCommand);
    }

    public IEnumerable<Link> GetMultipleLinks(IElementSelector selector, IElementSelector? scope = null)
    {
        QueryCommand<Link> queryCommand = new(selector, MapToLink, scope);
        return _documentQueryClientAccessor.DocumentQueryClient.QueryMany(queryCommand);
    }
}