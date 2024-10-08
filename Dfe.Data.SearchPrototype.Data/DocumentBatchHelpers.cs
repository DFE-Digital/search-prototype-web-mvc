﻿using Dfe.Data.SearchPrototype.Data.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Dfe.Data.SearchPrototype.Data;

class DocumentBatchHelpers
{
    private static readonly HttpClient httpClient = new HttpClient();

    public static IEnumerable<IEnumerable<dynamic>> SplitDataIntoBatches(IEnumerable<dynamic> source, int batchSize)
    {
        ArgumentNullException.ThrowIfNull(source);

        var list = source.ToList();
        for (int i = 0; i < list.Count; i += batchSize)
        {
            yield return list.Skip(i).Take(batchSize);
        }
    }

    public static string ConvertBatchToJson(IEnumerable<dynamic> batch)
    {
        var jsonDocuments = new
        {
            value = batch.Select(record => new
            {
                // Variable names are important here, and must be synchronised with names used on cognitive search (left)
                // and the auto-generated properties based on CSV column names (right)
                // COG_SEARCH_INDEX_NAME = record.SpreadsheetColumnName
                id = record.URN,
                ESTABLISHMENTNAME = record.EstablishmentName,
                TOWN = record.Town,
                TYPEOFESTABLISHMENTNAME = record.TypeOfEstablishmentName,
                STREET = record.Street,
                LOCALITY = record.Locality,
                ADDRESS3 = record.Address3,
                POSTCODE = record.Postcode,
                ESTABLISHMENTSTATUSNAME = record.EstablishmentStatusName,
                PHASEOFEDUCATION = record.PhaseOfEducation,
            })
        };
        return JsonConvert.SerializeObject(jsonDocuments, Formatting.Indented);
    }

    public static async Task SendBatchToSearchService(AzureSearchServiceDetails searchDetails, string json)
    {
        if (!searchDetails.IsValidServiceConfiguration)
        {
            throw new InvalidOperationException("Search details configuration missing");
        }

        var requestUri = $"https://{searchDetails.ServiceName}.search.windows.net/indexes/{searchDetails.IndexName}/docs/index?api-version=2020-06-30";
        var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("api-key", searchDetails.ApiKey);

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
}
