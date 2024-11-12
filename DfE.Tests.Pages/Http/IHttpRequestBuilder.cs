namespace DfE.Tests.Pages.Http;

public interface IHttpRequestBuilder
{
    public IHttpRequestBuilder SetPath(string path);
    public IHttpRequestBuilder AddQueryParameter(KeyValuePair<string, string> queryParameter);
    public IHttpRequestBuilder SetBody<T>(T value) where T : class;
    public HttpRequestMessage Build();
}
