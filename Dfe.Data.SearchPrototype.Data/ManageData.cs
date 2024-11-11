using Dfe.Data.SearchPrototype.Data.Configuration;
using Dfe.Data.SearchPrototype.Data.models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Dfe.Data.SearchPrototype.Data;

public class ManageData
{
    private const int BatchSize = 99; // max size for postcode lookup API
    private PostcodeLookupService _postcodeLookupService;
    private ILogger _logger;

    public ManageData(PostcodeLookupService postcodeLookupService, ILogger logger)
    {
        _postcodeLookupService = postcodeLookupService;
        _logger = logger;
    }

    public async Task ExtractAndUploadData(AzureSearchServiceDetails searchDetails, string filePath)
    {
        try
        {
            var records = GetData.ReadRecordsFromCsv(filePath);

            var batches = DocumentBatchHelpers.SplitDataIntoBatches(records, BatchSize);
            foreach (var batch in batches)
            {
                var postcodes = batch.Select(x => x.POSTCODE).Where(postcode => postcode != null).Cast<string>();
                try
                {
                    var geoLocations = await _postcodeLookupService.LookupPostcodes(postcodes);
                    var establishmentsOut = new { value = new List<EstablishmentOut>() };
                    foreach (var establishment in batch)
                    {
                        var geolocation = geoLocations?.FirstOrDefault(x => x?.postcode == establishment.POSTCODE);
                        if (geolocation != null)
                        {
                            var establishmentOut = new EstablishmentOut(latitude: geolocation.latitude, longitude: geolocation.longitude)
                            {
                                id = establishment.id,
                                ADDRESS3 = establishment.ADDRESS3,
                                POSTCODE = establishment.POSTCODE,
                                ESTABLISHMENTNAME = establishment.ESTABLISHMENTNAME,
                                ESTABLISHMENTSTATUSNAME = establishment.ESTABLISHMENTSTATUSNAME,
                                LOCALITY = establishment.LOCALITY,
                                PHASEOFEDUCATION = establishment.PHASEOFEDUCATION,
                                STREET = establishment.STREET,
                                TOWN = establishment.TOWN,
                                TYPEOFESTABLISHMENTNAME = establishment.TYPEOFESTABLISHMENTNAME,
                            };
                            establishmentsOut.value.Add(establishmentOut);
                        }
                        else { _logger.LogInformation($"No data for postcode {establishment.POSTCODE}"); }
                    }
                    string json = JsonSerializer.Serialize(establishmentsOut);

                    await DocumentBatchHelpers.SendBatchToSearchService(
                       searchDetails, json);
                }
                catch (Exception ex) {
                    // LookupPostcodes might have thrown
                };

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
