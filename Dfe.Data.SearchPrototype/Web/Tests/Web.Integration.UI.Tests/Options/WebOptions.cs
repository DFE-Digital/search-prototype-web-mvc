namespace DfE.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Options;

public sealed class WebOptions
{
    private const ushort DEFAULT_HTTPS_PORT = 443;
    public const string Key = "web";
    public string Scheme { get; set; } = null!;
    public string Domain { get; set; } = null!;
    public ushort Port { get; set; } = DEFAULT_HTTPS_PORT;
    public string? WebUri { private get; set; } = null;

    public string GetWebUri() =>
        !string.IsNullOrEmpty(WebUri) ? WebUri :
            Port == DEFAULT_HTTPS_PORT ? $"{Scheme}://{Domain}" : $"{Scheme}://{Domain}:{Port}";
}
