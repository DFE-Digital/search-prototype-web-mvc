﻿using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
using Dfe.Testing.Pages;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests;

public abstract class BaseHttpTest : IDisposable
{
    private static readonly TestServices services = new();
    private readonly IServiceScope _serviceScope;

    protected BaseHttpTest(ITestOutputHelper testOutputHelper)
    {
        _serviceScope = services.CreateServiceScopeResolver();
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

internal sealed class TestServices
{
    private readonly IServiceProvider _serviceProvider;
    internal TestServices()
    {
        IServiceCollection services = new ServiceCollection()
            .AddTransient<SearchComponent>()
            .AddTransient<NavigationBarComponent>()
            .AddTransient<HomePage>()
            .AddTransient<SearchResultsComponent>()
            .AddPages<Program>();

        // TODO delaying the creation of the program so it can be overwritten in a test
        _serviceProvider = services.BuildServiceProvider();
    }

    internal IServiceScope CreateServiceScopeResolver() => _serviceProvider.CreateScope();
}
