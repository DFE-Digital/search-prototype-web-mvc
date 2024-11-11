using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;
public interface IDocumentQueryClientAccessor
{
    IDocumentQueryClient DocumentQueryClient { get; set; }
}