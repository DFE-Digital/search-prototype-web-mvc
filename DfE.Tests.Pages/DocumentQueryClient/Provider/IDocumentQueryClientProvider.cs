namespace DfE.Tests.Pages.DocumentQueryClient.Provider;

public interface IDocumentQueryClientProvider
{
    Task<IDocumentQueryClient> CreateDocumentClientAsync(HttpRequestMessage httpRequestMessage);
}
