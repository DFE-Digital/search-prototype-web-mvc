namespace Dfe.Testing.Pages.Http;

internal sealed class HttpRequestBuilder : IHttpRequestBuilder
{
    private string _path = "/";
    private List<KeyValuePair<string, string>> _query = new();
    private object? _body = null;

    public IHttpRequestBuilder SetPath(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        _path = path;
        return this;
    }

    public IHttpRequestBuilder AddQueryParameter(KeyValuePair<string, string> query)
    {
        _query.Add(query);
        return this;
    }

    public IHttpRequestBuilder SetBody<T>(T value) where T : class
    {
        ArgumentNullException.ThrowIfNull(value);
        _body = value;
        return this;
    }

    public HttpRequestMessage Build()
    {
        UriBuilder uri = new()
        {
            Path = _path,
            Query = _query.ToList()
                .Aggregate(
                    new StringBuilder(), (init, queryPairs) => init.Append($"{queryPairs.Key}={queryPairs.Value}"))
                .ToString()
        };

        HttpRequestMessage requestMessage = new()
        {
            RequestUri = uri.Uri
        };

        if (_body != null)
        {
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(_body));
        }

        return requestMessage;
    }
}
