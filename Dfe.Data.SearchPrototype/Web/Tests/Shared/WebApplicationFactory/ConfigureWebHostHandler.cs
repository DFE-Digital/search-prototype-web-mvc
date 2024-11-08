using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.WebApplicationFactory;
public interface IConfigureWebHostHandler
{
    IWebHostBuilder Handle(IWebHostBuilder builder);
    void ConfigureWith(Action<IWebHostBuilder> configure);
}

public sealed class ConfigureWebHostHandler : IConfigureWebHostHandler
{
    private Action<IWebHostBuilder>? _configure;
    public void ConfigureWith(Action<IWebHostBuilder> configure) => _configure = configure;

    public IWebHostBuilder Handle(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        _configure?.Invoke(builder);
        return builder;
    }
}