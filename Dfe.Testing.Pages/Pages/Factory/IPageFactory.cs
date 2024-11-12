namespace Dfe.Testing.Pages.Pages.Factory;
public interface IPageFactory
{
    public Task<TPage> CreatePageAsync<TPage>(HttpRequestMessage httpRequest) where TPage : class;
}
