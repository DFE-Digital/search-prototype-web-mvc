using BoDi;
using TechTalk.SpecFlow;
using Microsoft.Extensions.Options;
using DfE.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Drivers;
using DfE.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Options;

namespace DfE.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Hooks
{
    [Binding]
    public class SpecFlowHooks
    {
        [BeforeTestRun]
        public static void BeforeTest(ObjectContainer container)
        {

            container.BaseContainer.RegisterInstanceAs(OptionsHelper.GetOptions<WebOptions>(WebOptions.Key));

            var driverOptions = OptionsHelper.GetOptions<WebDriverOptions>(WebDriverOptions.Key);
            if (string.IsNullOrEmpty(driverOptions.Value.DriverBinaryDirectory))
            {
                driverOptions.Value.DriverBinaryDirectory = Directory.GetCurrentDirectory();
            }
            container.BaseContainer.RegisterInstanceAs(driverOptions);
            var accessibilityOptions = OptionsHelper.GetOptions<AccessibilityOptions>(AccessibilityOptions.Key);
            accessibilityOptions.Value.CreateArtifactOutputDirectory();
            container.BaseContainer.RegisterInstanceAs(accessibilityOptions);
        }

        [BeforeScenario]
        public void CreateWebDriver(IObjectContainer container)
        {
            container.RegisterTypeAs<WebDriverFactory, IWebDriverFactory>();
            container.RegisterTypeAs<WebDriverContext, IWebDriverContext>();
        }
    }
}