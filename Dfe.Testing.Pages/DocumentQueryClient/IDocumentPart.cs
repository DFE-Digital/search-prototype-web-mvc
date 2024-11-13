﻿namespace Dfe.Testing.Pages.DocumentQueryClient;
public interface IDocumentPart
{
    string Text { get; set; }
    string GetAttribute(string attributeName);
    IDictionary<string, string> GetAttributes();
    IEnumerable<IDocumentPart> GetChildren();
    IDocumentPart? GetChild(IElementSelector selector);
}