using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Dfe.Data.SearchPrototype.Web.Tests.ViewTests;

public class IntegrationTestingWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>> _useCase = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>>();
            services.AddScoped<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>>(provider => _useCase.Object);
        });
    }
}
