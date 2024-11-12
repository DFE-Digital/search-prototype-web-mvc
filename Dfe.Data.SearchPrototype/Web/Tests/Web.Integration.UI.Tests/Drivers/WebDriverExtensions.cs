using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Xunit;

namespace DfE.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Drivers;

public static class WebDriverExtensions
{
    public static void ElementDoesNotExist(this IWebDriverContext context, Func<IWebElement> locate)
    {
        context.Wait.Timeout = TimeSpan.FromSeconds(4);
        Assert.ThrowsAny<WebDriverTimeoutException>(locate);
    }

    public static IWebElement UntilElementContainsText(this IWebElement element, IWebDriverContext context, string text)
    {
        _ = text ?? throw new ArgumentNullException(nameof(text));
        context.Wait.Message = $"Element did not contain text {text}";
        context.Wait.Until(t => element.Text.Contains(text));
        context.Wait.Message = string.Empty;
        return element;
    }

    public static IWebElement UntilElementTextIs(IWebElement element, IWait<IWebDriver> wait, string text)
    {
        _ = text ?? throw new ArgumentNullException(nameof(text));
        wait.Message = $"Element did not equal text {text}";
        wait.Until(t => element.Text.Contains(text));
        wait.Message = string.Empty;
        return element;
    }

    public static IWebElement UntilAriaExpandedIs(this IWait<IWebDriver> wait, bool isExpanded, By locator)
    {
        var element = wait.UntilElementExists(locator);
        wait.Until(_ => element.GetAttribute("aria-expanded") == (isExpanded ? "true" : "false"));
        return element;
    }
    public static IWebElement UntilElementExists(this IWait<IWebDriver> wait, By by) => wait.Until(ExpectedConditions.ElementExists(by));

    public static IWebElement UntilElementIsVisible(this IWait<IWebDriver> wait, By by) => wait.Until(ExpectedConditions.ElementIsVisible(by));

    public static IWebElement UntilElementIsClickable(this IWait<IWebDriver> wait, By by) => wait.Until(ExpectedConditions.ElementToBeClickable(by));
    public static IReadOnlyList<IWebElement> UntilMultipleElementsExist(this IWait<IWebDriver> wait, By by) => wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
    public static IReadOnlyList<IWebElement> UntilMultipleElementsVisible(this IWait<IWebDriver> wait, By by) => wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(by));
}
