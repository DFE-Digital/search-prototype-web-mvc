namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;

public interface IDomQueryClientFactory
{
    Task<IDomQueryClient> CreateClientFromHttpRequestAsync(HttpRequestMessage httpRequestMessage);
}
