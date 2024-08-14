using System.Text;

namespace Dfe.Data.SearchPrototype.Data;

class DocumentBatchHelpers
{
    private static readonly HttpClient httpClient = new HttpClient();

    public static async Task SendBatchToSearchService(string serviceName, string apiKey, string indexName, string json)
{
    var size = Encoding.UTF8.GetByteCount(json);
    var requestUri = $"https://{serviceName}.search.windows.net/indexes/{indexName}/docs/index?api-version=2020-06-30";
    var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
    {
        Content = new StringContent(json, Encoding.UTF8, "application/json")
    };
    request.Headers.Add("api-key", apiKey);

    try
    {
        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Documents uploaded successfully.");
        Console.WriteLine(responseContent);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to upload documents: {ex.Message}");
        //check payload size as Azure Search accepts either 1000docs or 16MB whatever comes first
        Console.WriteLine($"Payload size: {Encoding.UTF8.GetByteCount(json)} bytes");
    }
}

    public static IEnumerable<IEnumerable<dynamic>> SplitDataIntoBatches(IEnumerable<dynamic> source, int batchSize)
    {
        var list = source.ToList();
        for (int i = 0; i < list.Count; i += batchSize)
        {
            yield return list.Skip(i).Take(batchSize);
        }
    }

}
