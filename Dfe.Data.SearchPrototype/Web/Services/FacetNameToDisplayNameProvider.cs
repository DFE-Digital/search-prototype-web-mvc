namespace Dfe.Data.SearchPrototype.Web.Services;

public class FacetNameToDisplayNameProvider : INameKeyToDisplayNameProvider
{
    private Dictionary<string, string> _facetNameKeyToDisplayNameDictionary;

    public FacetNameToDisplayNameProvider(Dictionary<string, string> facetNameKeyToDisplayNameDictionary)
    {
        _facetNameKeyToDisplayNameDictionary = facetNameKeyToDisplayNameDictionary;
    }
    public string GetDisplayName(string nameKey)
    {
        return _facetNameKeyToDisplayNameDictionary.ContainsKey(nameKey)
            ? _facetNameKeyToDisplayNameDictionary[nameKey]
            : nameKey;
    }
}
