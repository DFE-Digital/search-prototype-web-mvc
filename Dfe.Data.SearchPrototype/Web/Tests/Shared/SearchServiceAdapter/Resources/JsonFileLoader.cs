using Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter.Options;
using Microsoft.Extensions.Options;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter.Resources
{
    public sealed class JsonFileLoader : IJsonFileLoader
    {
        private readonly DummySearchServiceAdapterOptions _options;

        public JsonFileLoader(IOptions<DummySearchServiceAdapterOptions> options)
        {
            _options = options.Value;
        }

        public Task<string> LoadJsonFile() => LoadJsonFile(_options.FileName!);

        public async Task<string> LoadJsonFile(string path)
        {
            string? rawJson = null;

            using (StreamReader sr = new StreamReader(path))
            {
                rawJson = await sr.ReadToEndAsync();
            }

            return rawJson;

        }
    }

    public interface IJsonFileLoader
    {
        Task<string> LoadJsonFile();

        Task<string> LoadJsonFile(string path);
    }
}
