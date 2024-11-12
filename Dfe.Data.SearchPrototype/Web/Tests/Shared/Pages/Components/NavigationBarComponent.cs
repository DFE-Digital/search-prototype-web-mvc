using DfE.Tests.Pages.DocumentQueryClient.Accessor;
using DfE.Tests.Pages.DocumentQueryClient.Selector;
using DfE.Tests.Pages.Pages.Components;
using DfE.Tests.Pages.Pages.Components.AnchorLink;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

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