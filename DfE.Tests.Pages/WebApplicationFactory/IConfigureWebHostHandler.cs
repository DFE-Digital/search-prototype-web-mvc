namespace DfE.Tests.Pages.WebApplicationFactory;
public interface IConfigureWebHostHandler
{
    IWebHostBuilder Handle(IWebHostBuilder builder);
    void ConfigureWith(Action<IWebHostBuilder> configure);
}