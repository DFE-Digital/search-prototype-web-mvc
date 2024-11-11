using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.Link;
public record Link(
    string link,
    string text,
    bool opensInNewTab);
