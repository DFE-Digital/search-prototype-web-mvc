namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;

public interface IDocumentQueryClientProvider
{
    Task<IDocumentQueryClient> CreateDocumentClientAsync(HttpRequestMessage httpRequestMessage);
}
