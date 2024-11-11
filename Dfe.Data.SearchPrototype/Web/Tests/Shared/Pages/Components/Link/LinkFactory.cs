using DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.Link;

public sealed class LinkFactory
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;

    public LinkFactory(IDocumentQueryClientAccessor documentQueryClientAccessor)
    {
        _documentQueryClientAccessor = documentQueryClientAccessor;
    }

    private IDocumentQueryClient DocumentQueryClient => _documentQueryClientAccessor.DocumentQueryClient;

    public Link CreateLink(
        IQuerySelector selector,
        IQuerySelector? scope = null)
    {
        return DocumentQueryClient.Query(
            new QueryCommand<Link>(
                selector,
                (documentPart) =>
                {
                    return new(
                            link: documentPart.GetAttribute("href"),
                            text: documentPart.Text.Trim(),
                            opensInNewTab: documentPart.GetAttribute("target") == "_blank");
                },
                scope));
    }
}