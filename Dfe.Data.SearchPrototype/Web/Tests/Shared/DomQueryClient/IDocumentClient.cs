namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;

public interface IDocumentClient
{
    string? GetText(string cssSelector);
    string? GetLink(string cssSelector);
    string? GetAttribute(string cssSelector, string attribute);
    IEnumerable<string?>? GetTexts(string cssSelector);
    bool ElementExists(string cssSelector);
    int GetCount(string criteria);
}