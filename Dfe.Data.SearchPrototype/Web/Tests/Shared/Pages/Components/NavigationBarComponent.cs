using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.Link;

public sealed class NavigationBarComponent : ComponentBase
{
    private readonly LinkQueryCommand _linkCommand;
    internal override IQuerySelector Container => new CssSelector("#navigation-bar");

    public NavigationBarComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        LinkQueryCommand linkCommand) : base(documentQueryClientAccessor)
    {
        _linkCommand = linkCommand;
    }

    public Link GetHome() => _linkCommand.GetLink(selector: new CssSelector("#home-link"), scope: Container);

    public Link GetHeading() => _linkCommand.GetLink(selector: new CssSelector("#navigation-bar-service-name-link"), scope: Container);
}