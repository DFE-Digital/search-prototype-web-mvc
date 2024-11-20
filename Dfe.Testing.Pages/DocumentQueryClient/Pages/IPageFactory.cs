namespace Dfe.Testing.Pages.DocumentQueryClient.Pages;
public interface IPageFactory
{
    public Task<TPage> CreatePageAsync<TPage>(HttpRequestMessage httpRequest) where TPage : class;
}