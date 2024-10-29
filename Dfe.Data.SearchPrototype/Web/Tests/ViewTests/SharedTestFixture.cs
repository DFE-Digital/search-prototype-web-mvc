using AngleSharp;
using AngleSharp.Io;
using AngleSharp.Io.Network;
using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Tests.PresentationLayerTests;
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
    protected const string _homeUri = "http://localhost";
    protected SearchPageModel _searchPage;
    protected Mock<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>> _useCase = new();

    public SharedTestFixture(WebApplicationFactory<Program> factory)
    {
        HttpClient client = CreateHost(factory).CreateClient();
        IBrowsingContext context = CreateBrowsingContext(client);
        _searchPage = new SearchPageModel(context);
    }

    private IBrowsingContext CreateBrowsingContext(HttpClient httpClient)
    {
        var config = AngleSharp.Configuration.Default
            .WithRequester(new HttpClientRequester(httpClient))
            .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

        return BrowsingContext.New(config);
    }

    private WebApplicationFactory<Program> CreateHost(WebApplicationFactory<Program> factory)
    {
        return factory.WithWebHostBuilder(
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
