using Dfe.Data.SearchPrototype.Data;

class Program
{
    private const int BatchSize = 1000;

    static async Task Main(string[] args)
    {
        //Azure Cognitive Search service details
        string serviceName = "s123d01-aisearch";
        string apiKey = "";
        string indexName = "establishments";
        //Local csv file path
        string filePath = @"C:\SearchPrototypeData\GIASextract\edubasealldata20240807.csv";

        var records = GetData.ReadRecordsFromCsv(filePath);

        var batches = DocumentBatchHelpers.SplitDataIntoBatches(records, BatchSize);
        foreach (var batch in batches)
        {
            string json = DocumentBatchHelpers.ConvertBatchToJson(batch);
            await DocumentBatchHelpers.SendBatchToSearchService(serviceName, apiKey, indexName, json);
        }
    }
}






