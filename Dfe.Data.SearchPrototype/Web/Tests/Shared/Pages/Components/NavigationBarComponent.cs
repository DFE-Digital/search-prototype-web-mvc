using Dfe.Testing.Pages.DocumentQueryClient.Accessor;
using Dfe.Testing.Pages.DocumentQueryClient.Selector;
using Dfe.Testing.Pages.Pages.Components;
using Dfe.Testing.Pages.Pages.Components.AnchorLink;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
public sealed class NavigationBarComponent : ComponentBase
{
    internal static ElementSelector Container => new("#navigation-bar");
    private readonly LinkMapper _linkCommand;

    public NavigationBarComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        LinkMapper linkCommand) : base(documentQueryClientAccessor)
    {
        _linkCommand = linkCommand;
    }

    public Link GetHome() => _linkCommand.GetLink(selector: new ElementSelector("#home-link"), scope: Container);

    public Link GetHeading() => _linkCommand.GetLink(selector: new ElementSelector("#navigation-bar-service-name-link"), scope: Container);
}
