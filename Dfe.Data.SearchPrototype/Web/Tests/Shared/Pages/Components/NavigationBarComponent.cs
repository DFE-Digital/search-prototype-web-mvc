using Dfe.Testing.Pages.DocumentQueryClient.Accessor;
using Dfe.Testing.Pages.DocumentQueryClient.Selector;
using Dfe.Testing.Pages.Pages.Components;
using Dfe.Testing.Pages.Pages.Components.AnchorLink;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public sealed class NavigationBarComponent : ComponentBase
{
    internal static IQuerySelector Container => new ElementSelector("#navigation-bar");
    private readonly LinkQueryCommand _linkCommand;

    public NavigationBarComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        LinkQueryCommand linkCommand) : base(documentQueryClientAccessor)
    {
        _linkCommand = linkCommand;
    }

    public Link GetHome() => _linkCommand.GetLink(selector: new ElementSelector("#home-link"), scope: Container);

    public Link GetHeading() => _linkCommand.GetLink(selector: new ElementSelector("#navigation-bar-service-name-link"), scope: Container);
}