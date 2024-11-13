namespace Dfe.Testing.Pages.DocumentQueryClient.Provider;
public interface IDocumentQueryClientProvider
{
    Task<IDocumentQueryClient> CreateDocumentClientAsync(HttpRequestMessage httpRequestMessage);
}