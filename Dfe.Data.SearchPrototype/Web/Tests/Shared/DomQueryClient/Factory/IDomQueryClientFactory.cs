using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;

public interface IDomQueryClientFactory
{
    Task<IDomQueryClient> CreateAsync(string path);
}
