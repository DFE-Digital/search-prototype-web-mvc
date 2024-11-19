namespace Dfe.Testing.Pages.DocumentQueryClient;
public interface IDocumentPart
{
    string Text { get; set;  }
    string? GetAttribute(string attributeName);
    IDictionary<string, string> GetAttributes();
    List<IDocumentPart> GetChildren();
    List<IDocumentPart> GetChildren(IElementSelector selector);
    IDocumentPart? GetChild(IElementSelector selector);
    void Click();
}