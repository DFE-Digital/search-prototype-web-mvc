# Who is this library for

This library is targeted to support .NET Developers and Testers

## What problems is the library solving

- Removing dependencies on specific test tools in Pages and Tests allowing the library to abstract

- By reducing coupling to test tools in Pages and Tests allows sharing Pages among different types of tests (Presentation layer tests, Integration Tests, EndToEnd tests)

- Demonstrate patterns for reuseable PageModels using PageComponents and [GDSComponents](https://design-system.service.gov.uk/components/) as they should be used as the building blocks for all CivilService web applications

## Using the library

The Dfe.Testing.Pages library supports the below providers

- `AngleSharp`

```cs
services.AddAngleSharp<TApplicationProgram>(); // TApplicationProgram is your .NET Program class for your Web Application
```

- `Selenium.WebDriver`

```cs
services.AddWebDriver();
```

In order to use the library you will need to setup [DependencyInjection](TODO LINK) inside of your tests and register the provider you want for that test suite. Below is an example of how you setup DependencyInjection.

```cs
// This is a Singleton that wraps the DependencyInjection container allowing for the services to be configured and built once.
// A scope is a child scope of the root DependencyInjection container, when you resolve through a scope, after you dispose of the scope - `Scoped` dependencies are disposed of.
.AddSingleton<TImplementation>()
.AddScoped<TInterface, TImplementation>();
_scope.Resolve<TInterface>();
_scope.Dispose(); // My scoped instance gets disposed, my Singleton remains

internal sealed class DependencyInjection
{
    private static readonly DependencyInjection _instance = new();
    private readonly IServiceProvider _serviceProvider;
    static DependencyInjection()
    {
    }

    private DependencyInjection()
    {
        IServiceCollection services = new ServiceCollection()
            .AddPages()
            // ToAddAngleSharp .AddAngleSharp<Program>();
            // ToAddWebDriver .AddWebDriver();
        _serviceProvider = services.BuildServiceProvider();
    }

    public static DependencyInjection Instance
    {
        get
        {
            return _instance;
        }
    }

    internal IServiceScope CreateScope() => _serviceProvider.CreateScope();
}

// Separately you want to consume this in your BaseTestClass

public abstract class BaseTest : IDisposable
{
    private readonly IServiceScope _serviceScope;

    protected BaseHttpTest()
    {
        _serviceScope = DependencyInjection.Instance.CreateScope();
    }

    protected T GetTestService<T>()
        => _serviceScope.ServiceProvider.GetService<T>()
            ?? throw new ArgumentNullException($"Unable to resolve type {typeof(T)}");

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _serviceScope.Dispose();
    }
}

// Any test class you inherit from BaseTest gets access to the container and a new scope is created per test

public sealed class MyTestClass : BaseTest
{
  [Fact]
  public async Task MyTest()
  {
    IPageFactory pageFactory = GetTestService<IPageFactory>(); // is available
  }
}
```


- Adding your application components into DI
- Example test

## Common terms and abstractions

- `IDocumentQueryClientProvider`
- `IDocumentQueryClient`
- `PagePartBase`
- `IPageFactory`

## PageModels should be structued

- Custom components that an application uses
- GDSComponents that we provide

## Writing an PageModel for an application

## Configuring a provider