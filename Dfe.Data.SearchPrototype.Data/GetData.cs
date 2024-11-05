using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Dfe.Data.SearchPrototype.Data;

public class GetData
{
    public static List<Establishment> ReadRecordsFromCsv(string filePath)
    {
        var records = new List<Establishment>();

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            BadDataFound = null
        }))
        {
            // Reading records as dynamic objects
            while (csv.Read())
            {
                var record = csv.GetRecord<Establishment>();
                records.Add(record);
            }
            return records;
        }
    }
}