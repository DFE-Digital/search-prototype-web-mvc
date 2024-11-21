# Who is this library for

This library is targeted to support .NET Developers and Testers

## What problems is the library solving

- Removing dependencies on specific test tools in Pages and Tests allowing the library to abstract

- By reducing coupling to test tools in Pages and Tests allows sharing Pages among different types of tests (Presentation layer tests, Integration Tests, EndToEnd tests)

- Demonstrate patterns for reuseable PageModels using PageComponents and [GDSComponents](https://design-system.service.gov.uk/components/) as they should be used as the building blocks for all CivilService web applications

## Common terms and abstractions

- `IDocumentQueryClientProvider`
- `IDocumentQueryClient`
- `PagePartBase`
- `IPageFactory`

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
// This uses the Singleton pattern that wraps the DependencyInjection container allowing for the services to be configured and built once. 
// The `IServiceProvider` once built, is delegated responsibility for creating registered implementations of types and managing their lifetimes.

// An `IServiceScope` is a child scope of the root DependencyInjection container, when you resolve through a scope, after you dispose of the scope - `Scoped` dependencies are disposed of.
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

## Creating and using an application component

When building PageModels you want to:

- Add your types into your tests DependencyInjection

```cs
    services....
    services.AddTransient<HomePage>();
```

- Compose your PageModel of other PageComponents

```cs

services.AddTransient<SearchComponent>();
services.AddTransient<FilterComponent>();

public sealed class HomePage
{
    public HomePage(
        NavigationBarComponent navBar,
        SearchComponent search, 
        FilterComponent filter)
    {
        Search = search ?? throw new ArgumentNullException(nameof(search));
        Filter = filter ?? throw new ArgumentNullException(nameof(filter));
        NavBar = navBar ?? throw new ArgumentNullException(nameof(navBar));
    }

    // Can reuse these across PageModels
    public NavigationBarComponent NavBar { get; }
    public SearchComponent Search { get; }
    public FilterComponent Filter { get; }
}
```

- Expose the types that your tests need, which are not coupled to a testing library e.g


```cs
homePage.GetHeading().Should().Be("Heading"); 

public sealed class HomePage
{
    public string GetHeading() => ...
}
```

## You could expose a GDSComponent

```cs
// GDSComponent provided by the library
GDSTextInput textInput = new()
{
    Name = "searchKeyWord",
    Value = "",
    PlaceHolder = "Search by keyword",
    Type = "text"
};

homePage.TextInput.Should().Be(textInput);

public sealed class HomePage
{
    
    public GDSTextInput  GetSearchInput() => _textInputFactory.Create();
}

public record GDSTextInput
{
    public required string Name { get; init; }
    public required string Value { get; init; }
    public required string? PlaceHolder { get; init; } = null;
    public required string? Type { get; init; } = null;
}

```

## or a custom application type

```cs

// CUSTOM COMPLEX APPLICATION TYPE
public record Facet(string Name, IEnumerable<FacetValue> FacetValues);
public record FacetValue(string Label, string Value);

homePage.GetDisplayedFacets().Should().Be(new[]
{
    new Facet
    (
        Name: "Facet name",
        FacetValues: []
    ),
    new Facet
    (
        Name: "Facet name",
        FacetValues: []
    )
})

public sealed class HomePage
{
    public IEnumerable<Facet> GetDisplayedFacets()
        => _formFactory.Get().FieldSets
                .Select(
                    (fieldSet) => new Facet(
                        Name: fieldSet.Legend,
                        FacetValues: fieldSet.Checkboxes.Select(
                         (checkbox) => new FacetValue(checkbox.Label,   checkbox.Value))));
}

```

## Using your PageModels

When using the PageModels you want to create them using the `PageFactory` which sets up your pages to use the `IDocumentQueryClient` configured.

```cs
public sealed class MyTestClass : BaseTest{

[Fact]
public async Task MyTest()
{
    HttpRequestMessage homePageRequest = new()
    {
        Uri = new("/")
    }
    HomePage homePage = await GetTestService<IPageFactory>().CreatePageAsync<HomePage>(homePageRequest);
}
}

```