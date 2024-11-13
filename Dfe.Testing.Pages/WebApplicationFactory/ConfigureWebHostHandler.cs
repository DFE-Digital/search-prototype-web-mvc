namespace Dfe.Testing.Pages.WebApplicationFactory;
internal sealed class ConfigureWebHostHandler : IConfigureWebHostHandler
{
    private readonly List<Action<IWebHostBuilder>> _configure = [];
    public void ConfigureWith(Action<IWebHostBuilder> configure)
        => _configure.Add(
                configure ?? throw new ArgumentNullException(nameof(_configure)));

    public IWebHostBuilder Handle(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        _configure.ToList().ForEach(
            (configureHandler) => configureHandler?.Invoke(builder));
        return builder;
    }
}