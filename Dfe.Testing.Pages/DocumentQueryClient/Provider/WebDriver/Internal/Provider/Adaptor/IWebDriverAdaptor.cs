using OpenQA.Selenium;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.Provider.Adaptor;
public interface IWebDriverAdaptor : IApplicationNavigator, IDisposable, IAsyncDisposable
{
    Task StartAsync();
    System.Net.Cookie? GetCookie(string cookieName);
    IEnumerable<System.Net.Cookie> GetCookies();
    Task TakeScreenshotAsync();
    // TODO need to adapt this to hide Selenium and return something mapped -- NOTE will need to be able to Find from the element
    // TODO extend to include FindOptions per request?
    IWebElement FindElement(IElementSelector selector);
    IReadOnlyCollection<IWebElement> FindElements(IElementSelector selector);
    //TODO something to mock a request?
}