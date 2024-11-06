namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;

public interface IDocumentClientFactory
{
    Task<IDocumentClient> CreateDocumentClientAsync(HttpRequestMessage httpRequestMessage);
}
