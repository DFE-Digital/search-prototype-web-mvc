using Dfe.Testing.Pages.DocumentQueryClient;
using Dfe.Testing.Pages.DocumentQueryClient.Accessor;
using Dfe.Testing.Pages.DocumentQueryClient.Pages;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.AnchorLink;
using Dfe.Testing.Pages.DocumentQueryClient.Selector;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
public sealed class NavigationBarComponent : PagePartBase
{
    internal static ElementSelector Container => new("#navigation-bar");
    internal static QueryRequest Heading => new(
        query: new ElementSelector("#navigation-bar-service-name-link"),
        scope: Container);

    internal static QueryRequest Home => new QueryRequest(query: new ElementSelector("#home-link"), scope: Container);

    private readonly AnchorLinkComponentFactory _linkCommand;

    public NavigationBarComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        AnchorLinkComponentFactory linkCommand) : base(documentQueryClientAccessor)
    {
        _linkCommand = linkCommand;
    }

    public AnchorLink GetHome() => _linkCommand.Get(Home);

    public AnchorLink GetHeading() => _linkCommand.Get(Heading);
}
