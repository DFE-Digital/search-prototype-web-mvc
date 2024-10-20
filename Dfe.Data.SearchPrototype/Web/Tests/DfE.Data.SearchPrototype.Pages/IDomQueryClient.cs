using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Net.Http.Headers;

namespace DfE.Data.SearchPrototype.Pages;

//NOTE these may go into a Tests.Infrastructure library

public interface IDomQueryClient
{
    string GetText(string cssSelector);
    string? GetLink(string cssSelector);
    string? GetAttribute(string cssSelector, string attribute);
    IEnumerable<string> GetTexts(string cssSelector);
}

public class WebDriverDomQueryClient : IDomQueryClient
{
    private readonly IWebDriverContext _webDriverContext;

    internal WebDriverDomQueryClient(IWebDriverContext webDriverContext)
    {
        _webDriverContext = webDriverContext;
    }

    public virtual string GetText(string cssSelector)
        => Find(cssSelector).Text ?? string.Empty;

    public virtual IEnumerable<string> GetTexts(string cssSelector)
    => FindAll(cssSelector)!
        .Select(x => x.Text ?? string.Empty);

    public virtual string? GetAttribute(string cssSelector, string attribute)
    {
        ArgumentException.ThrowIfNullOrEmpty(attribute);
        return Find(cssSelector).GetAttribute(attribute);
    }

    public virtual string? GetLink(string cssSelector)
        => GetAttribute(cssSelector, "href");

    internal virtual IWebElement Find(string cssSelector)
        => FindAll(cssSelector)!
            .ThrowIfNullOrEmpty()
            .ThrowIfMultiple()
            .Single();

    internal virtual IEnumerable<IWebElement>? FindAll(string cssSelector)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(cssSelector));
        return _webDriverContext.Driver.FindElements(By.CssSelector(cssSelector))
            ?? Enumerable.Empty<IWebElement>();
    }
}

internal class AngleSharpQueryClient : IDomQueryClient
{
    private readonly IHtmlDocument _htmlDocument;
    private AngleSharpQueryClient(IHtmlDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        _htmlDocument = document;
    }

    internal static async Task<AngleSharpQueryClient> CreateAsync(HttpResponseMessage response)
    {
        var document = await HtmlHelpers.GetDocumentAsync(response);
        return new(document);
    }
    public virtual string GetText(string cssSelector)
        => Find(cssSelector).TextContent.Trim();

    public virtual IEnumerable<string> GetTexts(string cssSelector)
        => FindAll(cssSelector)
            .Select(t => t.TextContent.Trim());

    public virtual string? GetAttribute(string cssSelector, string attribute)
    {
        ArgumentException.ThrowIfNullOrEmpty(attribute);
        return Find(cssSelector).GetAttribute(attribute);
    }
    public virtual string? GetLink(string cssSelector) => GetAttribute(cssSelector, "href");

    private IElement Find(string cssSelector)
        => FindAll(cssSelector)
            .ThrowIfMultiple()
            .Single();

    private IEnumerable<IElement> FindAll(string cssSelector)
    {
        ArgumentException.ThrowIfNullOrEmpty(cssSelector);
        return _htmlDocument.QuerySelectorAll(cssSelector)
            .ThrowIfNullOrEmpty();
    }

}

internal static class HtmlHelpers
{
    internal static async Task<IHtmlDocument> GetDocumentAsync(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var config = Configuration.Default.WithDefaultLoader();
        var document = await
            BrowsingContext.New(config)
                    .OpenAsync(ResponseFactory, CancellationToken.None);

        return (IHtmlDocument)document;
        void ResponseFactory(VirtualResponse htmlResponse)
        {
            htmlResponse
                .Address(response.RequestMessage!.RequestUri)
                .Status(response.StatusCode);

            MapHeaders(response.Headers);
            MapHeaders(response.Content.Headers);
            htmlResponse.Content(content);
            void MapHeaders(HttpHeaders headers)
            {
                foreach (var header in headers)
                {
                    foreach (var value in header.Value)
                    {
                        htmlResponse.Header(header.Key, value);
                    }
                }
            }
        }
    }
}

internal static class CollectionExtensions
{
    internal static IEnumerable<T> ThrowIfNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        if (!collection.Any())
        {
            throw new ArgumentException("Collection is empty");
        }
        return collection;
    }

    internal static IEnumerable<T> ThrowIfMultiple<T>(this IEnumerable<T> collection)
    {
        int collectionCount = collection.Count();
        if (collectionCount > 1)
        {
            throw new ArgumentException($"Collection count: {collectionCount} is more than 1");
        }
        return collection;
    }
}


public interface IWebDriverContext : IDisposable
{
    IWebDriver Driver { get; }
    IWait<IWebDriver> Wait { get; }
    void GoToUri(string path);
    void GoToUri(string baseUri, string path);
}

