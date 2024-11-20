namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.AnchorLink;
public sealed class AnchorLinkFactory : ComponentFactoryBase<AnchorLink>
{
    public AnchorLinkFactory(IDocumentQueryClientAccessor documentQueryClientAccessor) : base(documentQueryClientAccessor)
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
        QueryRequest queryRequest = new()
        {
            Query = request?.Query ?? new ElementSelector("a"),
            Scope = request?.Scope
        };

        return DocumentQueryClient.QueryMany(
            args: queryRequest,
            mapper: MapToLink)
            .ToList();
    }
}