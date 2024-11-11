using DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public sealed class NavigationBarComponent
{
    // TODO page needs to expose the link in a more defined GetHome() or IEnumerable<Link>
    //private IDictionary<string, >
    // TODO think on this IComponentFactory
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;
    private static IQuerySelector Container => new CssSelector("#navigation-bar");

    public NavigationBarComponent(IDocumentQueryClientAccessor documentQueryClientAccessor)
    {
        ArgumentNullException.ThrowIfNull(documentQueryClientAccessor);
        _documentQueryClientAccessor = documentQueryClientAccessor;
    }

    public Link GetHome()
        => _documentQueryClientAccessor.DocumentQueryClient.Query(
            new QueryCommand<Link>(
                query: new CssSelector("#home-link"),
                scope: Container,
                processor:
                    (part) => new(
                        part.GetAttribute("href"),
                        part.Text.Trim(),
                        opensInNewTab: part.GetAttribute("target") == "_blank")));

    public Link GetHeading()
        => _documentQueryClientAccessor.DocumentQueryClient.Query(
            new QueryCommand<Link>(
                query: new CssSelector("#navigation-bar-service-name-link"),
                scope: Container,
                processor:
                    (part) => new(
                        part.GetAttribute("href"),
                        part.Text.Trim(),
                        opensInNewTab: part.GetAttribute("target") == "_blank")));
}