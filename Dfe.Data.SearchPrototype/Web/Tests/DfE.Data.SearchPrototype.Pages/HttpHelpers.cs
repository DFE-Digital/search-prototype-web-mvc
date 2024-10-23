namespace DfE.Data.SearchPrototype.Pages;

public static class HttpHelpers
{
    public static async Task<Response> GetHttpResponseAsync(this HttpClient client, string path)
    {
        HttpResponseMessage response = await client.GetAsync(path);
        AngleSharpQueryClient domQueryClient = await AngleSharpQueryClient.CreateAsync(response);
        return new(httpResponseMessage: response, domQueryClient: domQueryClient);
    }
}

public sealed class Response
{
    public Response(HttpResponseMessage? httpResponseMessage, IDomQueryClient domQueryClient)
    {
        ArgumentNullException.ThrowIfNull(domQueryClient);
        HttpResponseMessage = httpResponseMessage;
        DomQueryClient = domQueryClient;
    }

    public IDomQueryClient DomQueryClient { get; }
    public HttpResponseMessage? HttpResponseMessage { get; }
}