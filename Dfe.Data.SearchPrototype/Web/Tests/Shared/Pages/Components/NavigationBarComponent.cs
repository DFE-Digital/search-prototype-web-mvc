using Dfe.Testing.Pages.DocumentQueryClient;
using Dfe.Testing.Pages.DocumentQueryClient.Accessor;
using Dfe.Testing.Pages.DocumentQueryClient.Pages;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.AnchorLink;
using Dfe.Testing.Pages.DocumentQueryClient.Selector;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
public sealed class NavigationBarComponent : PagePartBase
{
    internal static ElementSelector Container => new("#navigation-bar");

    private readonly AnchorLinkFactory _linkCommand;

    public NavigationBarComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        AnchorLinkFactory linkCommand) : base(documentQueryClientAccessor)
    {
        _linkCommand = linkCommand;
    }

    public IEnumerable<AnchorLink> GetLinks() => _linkCommand.GetMany(new QueryRequest() { Scope = Container });
}
