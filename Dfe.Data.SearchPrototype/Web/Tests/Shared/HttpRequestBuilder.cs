using System.Text;
using static Dfe.Data.SearchPrototype.Web.Tests.Shared.Constants;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared;

public sealed class HttpRequestBuilder
{
    private string? _path = Routes.HOME;
    private List<KeyValuePair<string, string>> _query = new();

    public HttpRequestBuilder SetPath(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        _path = path;
        return this;
    }

    public HttpRequestBuilder AddQuery(KeyValuePair<string, string> query)
    {
        _query.Add(query);
        return this;
    }

    public HttpRequestMessage Build()
    {
        UriBuilder uri = new()
        {
            Path = _path,
            Query = _query.ToList()
            .Aggregate(new StringBuilder(), (init, queryPairs) => init.Append($"{queryPairs.Key}={queryPairs.Value}"))
                .ToString()
        };

        return new HttpRequestMessage()
        {
            RequestUri = uri.Uri
        };
    }
}
