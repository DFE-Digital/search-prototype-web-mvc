using DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.Link;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.Link;

public sealed class NavigationBarComponent : ComponentBase
{
    private static IQuerySelector Container => new CssSelector("#navigation-bar");

    public NavigationBarComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        LinkFactory linkFactory) : base(documentQueryClientAccessor, linkFactory)
    {
    }

    public Link GetHome() 
        => LinkFactory.CreateLink(
            selector: new CssSelector("#home-link"),
            scope: Container);

    public Link GetHeading()
        => LinkFactory.CreateLink(
            selector: new CssSelector("#navigation-bar-service-name-link"),
            scope: Container);
}