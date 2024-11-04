using AngleSharp;
using AngleSharp.Io;
using AngleSharp.Io.Network;
using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.ViewTests;

public class SharedTestFixture : IClassFixture<WebApplicationFactory<Dfe.Data.SearchPrototype.Web.Program>>
{
    protected readonly HttpClient _client;
    protected readonly IBrowsingContext _context;
    protected readonly WebApplicationFactory<Program> _factory;
    protected Mock<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>> _useCase = new();

    public SharedTestFixture(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = CreateHost().CreateClient();
        _context = CreateBrowsingContext(_client);
    }

    private IBrowsingContext CreateBrowsingContext(HttpClient httpClient)
    {
        var config = AngleSharp.Configuration.Default
            .WithRequester(new HttpClientRequester(httpClient))
            .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

        return BrowsingContext.New(config);
    }

    private WebApplicationFactory<Program> CreateHost()
    {
        return _factory.WithWebHostBuilder(
            (IWebHostBuilder builder) =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>>();
                    services.AddScoped<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>>(provider => _useCase.Object);
                });
            }
        );
    }
}
