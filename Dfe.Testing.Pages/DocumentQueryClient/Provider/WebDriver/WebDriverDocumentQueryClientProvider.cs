using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver;

internal sealed class WebDriverDocumentQueryClientProvider : IDocumentQueryClientProvider
{
    public Task<IDocumentQueryClient> CreateDocumentClientAsync(HttpRequestMessage httpRequestMessage)
    {
        throw new NotImplementedException();
    }
}
