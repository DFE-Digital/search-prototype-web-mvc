using Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests;

public class BaseEndToEndTest : IDisposable
{
    private readonly IServiceScope _serviceScope;

    protected BaseEndToEndTest(ITestOutputHelper testOutputHelper)
    {
        _serviceScope = Services.Instance.CreateScope();
        TestOutputHelper = testOutputHelper;
    }

    protected ITestOutputHelper TestOutputHelper { get; }

    protected T GetTestService<T>()
        => _serviceScope.ServiceProvider.GetService<T>()
            ?? throw new ArgumentNullException($"Unable to resolve type {typeof(T)}");

    public void Dispose()
    {
        _serviceScope.Dispose();
        GC.SuppressFinalize(this);
    }
}
