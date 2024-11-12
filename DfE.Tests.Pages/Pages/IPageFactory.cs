namespace DfE.Tests.Pages.Pages;
public interface IPageFactory
{
    public Task<TPage> CreatePageAsync<TPage>(HttpRequestMessage httpRequest) where TPage : class;
}
