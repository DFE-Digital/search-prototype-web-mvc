using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Data.SearchPrototype.Data.Configuration;

public sealed class AzureSearchServiceDetails
{
    public string? ServiceName { get; set; }
    public string? IndexName { get; set; }
    public string? ApiKey { get; set; }

    public bool IsValidServiceConfiguration =>
        !string.IsNullOrWhiteSpace(ServiceName) &&
        !string.IsNullOrWhiteSpace(IndexName) &&
        !string.IsNullOrWhiteSpace(ApiKey);
}
