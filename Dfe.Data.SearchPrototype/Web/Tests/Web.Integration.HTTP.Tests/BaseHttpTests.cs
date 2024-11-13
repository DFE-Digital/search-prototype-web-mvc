using Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests;
public abstract class BaseHttpTest : IDisposable
{
    private readonly IServiceScope _serviceScope;

    protected BaseHttpTest(ITestOutputHelper testOutputHelper)
    {
        _serviceScope = TestServices.Instance.CreateScope();
        TestOutputHelper = testOutputHelper;
    }

    protected ITestOutputHelper TestOutputHelper { get; }

    protected T GetTestService<T>()
        => _serviceScope.ServiceProvider.GetService<T>()
            ?? throw new ArgumentNullException($"Unable to resolve type {typeof(T)}");

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _serviceScope.Dispose();
    }
}