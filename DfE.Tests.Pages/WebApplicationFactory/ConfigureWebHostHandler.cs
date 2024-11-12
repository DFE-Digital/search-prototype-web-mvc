namespace DfE.Tests.Pages.WebApplicationFactory;
internal sealed class ConfigureWebHostHandler : IConfigureWebHostHandler
{
    private List<Action<IWebHostBuilder>> _configure = new();
    public void ConfigureWith(Action<IWebHostBuilder> configure)
        => _configure.Add(
                configure ?? throw new ArgumentNullException(nameof(_configure)));

    public IWebHostBuilder Handle(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        _configure.ToList().ForEach(t =>
        {
            t?.Invoke(builder);
        });
        return builder;
    }
}
