using CsvHelper;
using Dfe.Data.SearchPrototype.Data.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Data.SearchPrototype.Data;

public class ManageData
{
    private const int BatchSize = 1000;
    public static async Task ExtractAndUploadData(AzureSearchServiceDetails searchDetails, string filePath)
    {
        try
        {
            var records = GetData.ReadRecordsFromCsv(filePath);

            var batches = DocumentBatchHelpers.SplitDataIntoBatches(records, BatchSize); ;
            foreach (var batch in batches)
            {
                string json = DocumentBatchHelpers.ConvertBatchToJson(batch);

                await DocumentBatchHelpers.SendBatchToSearchService(
                   searchDetails, json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
