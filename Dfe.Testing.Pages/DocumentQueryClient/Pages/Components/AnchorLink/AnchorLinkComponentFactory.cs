namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.AnchorLink;
public sealed class AnchorLinkComponentFactory : ComponentFactoryBase<AnchorLink>
{
    public AnchorLinkComponentFactory(IDocumentQueryClientAccessor documentQueryClientAccessor) : base(documentQueryClientAccessor)
    {
    }

    private static Func<IDocumentPart, AnchorLink> MapToLink =>
        (documentPart)
            => new AnchorLink 
            { 
                LinkValue = documentPart.GetAttribute("href")!, 
                Text = documentPart.Text.Trim(), 
                OpensInNewTab = documentPart.GetAttribute("target") == "_blank" 
            };

    public override List<AnchorLink> GetMany(QueryRequest? request = null)
    {
        IElementSelector? scope = request?.Scope;
        QueryRequest queryRequest =
            new(
                query: request?.Query ?? new ElementSelector("a"),
                scope);

        return DocumentQueryClient.QueryMany(
            args: queryRequest,
            mapper: MapToLink)
            .ToList();
    }
}