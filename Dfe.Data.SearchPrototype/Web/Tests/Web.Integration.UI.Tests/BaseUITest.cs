using Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests
{
    public class BaseUITest : IDisposable
    {
        private IServiceScope _serviceScope;
        public ITestOutputHelper TestOutputHelper { get; }
        
        public BaseUITest(ITestOutputHelper testOutputHelper)
        {
            _serviceScope = Services.Instance.CreateScope();
            TestOutputHelper = testOutputHelper;
        }

        protected T GetTestService<T>()
        => _serviceScope.ServiceProvider.GetService<T>()
            ?? throw new ArgumentNullException($"Unable to resolve type {typeof(T)}");

        public void Dispose()
        {
            _serviceScope.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
