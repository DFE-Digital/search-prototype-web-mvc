using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using System.Globalization;
using Dfe.Data.SearchPrototype.Data;
using Dfe.Data.SearchPrototype.Infrastructure.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;

class Program
{

    private const int BatchSize = 1000;//max payload is 1000docs or 16MB whatever comes first

    static async Task Main(string[] args)
    {
        //var run = new Program();
        //await run.MethodName();
        //Azure Cognitive Search service details
        string serviceName = "s123d01-aisearch";
        string apiKey = "";
        string indexName = "test-index";
        string filePath = @"C:\SearchPrototypeData\GIASextract\edubasealldata20240807.csv";
        var records = new List<dynamic>();

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true, // Assumes the CSV has headers
            TrimOptions = TrimOptions.Trim, // Trims whitespace
            BadDataFound = null // Ignores bad data
        }))
        {
            // Reading records as dynamic objects
            while (csv.Read())
            {
                var record = csv.GetRecord<dynamic>();
                records.Add(record);
            }

            // Split records into batches and process each batch
            var batches = DocumentBatchHelpers.SplitDataIntoBatches(records, BatchSize);

            foreach (var batch in batches)
            {
                var jsonDocuments = new
                {
                    value = batch.Select(record => new
                    {
                        id = record.URN,
                        ESTABLISHMENTNAME = record.EstablishmentName,
                        TOWN = record.Town,
                        TYPEOFESTABLISHMENTNAME = record.TypeOfEstablishmentName,
                        ISPRIMARY = "",
                        ISSECONDARY = "",
                        ISPOST16 = "",
                        STREET = record.Street,
                        LOCALITY = record.Locality,
                        ADDRESS3 = record.Address3,
                        POSTCODE = record.Postcode,
                        ESTABLISHMENTSTATUSCODE = record.EstablishmentStatusCode
                    })
                };

                // Convert the records to JSON
                string json = JsonConvert.SerializeObject(jsonDocuments, Formatting.Indented);

                await DocumentBatchHelpers.SendBatchToSearchService(serviceName, apiKey, indexName, json);
            }

        }
    }
    }


    
