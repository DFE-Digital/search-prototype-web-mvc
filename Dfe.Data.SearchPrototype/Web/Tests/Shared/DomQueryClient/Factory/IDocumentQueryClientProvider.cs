namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;

public interface IDocumentQueryClientProvider
{
    Task<IDocumentClient> CreateDocumentClientAsync(HttpRequestMessage httpRequestMessage);
}
