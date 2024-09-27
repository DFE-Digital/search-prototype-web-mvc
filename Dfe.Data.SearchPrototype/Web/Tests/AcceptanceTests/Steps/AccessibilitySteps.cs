using Deque.AxeCore.Selenium;
using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;
using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Extensions;
using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Options;
using Dfe.Data.SearchPrototype.Web.Tests.AcceptanceTests;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using FluentAssertions;
using Microsoft.Extensions.Options;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Steps
{

    [Binding]
    public sealed class AccessibilitySteps : IClassFixture<WebApplicationFactoryFixture<Program>>
    {
        private readonly AccessibilityOptions _options;
        private readonly ITestOutputHelper _logger;
        private readonly IWebDriverContext _driverContext;
        private readonly HomePage _searchPage;
        private readonly WebDriverSessionOptions _sessionOptions;
        private readonly HttpClient _httpClient;
        private readonly WebApplicationFactoryFixture<Program> _factory;

        private Dictionary<string, string> _pageNameToUrlConverter = new Dictionary<string, string>()
        {
            { "home", "/" },
            { "search results", "/?searchKeyWord=Academy" }
        };

        public AccessibilitySteps(
            WebApplicationFactoryFixture<Program> factory,
            IOptions<AccessibilityOptions> options,
            ITestOutputHelper logger,
            IWebDriverContext driverContext,
            HomePage searchPage,
            WebDriverSessionOptions sessionOptions
        )
        {
            _factory = factory;
            _httpClient = _factory.CreateDefaultClient();

            _driverContext = driverContext;
            _searchPage = searchPage;
            _logger = logger;
            _options = options.Value;
            _sessionOptions = sessionOptions;
        }

        [StepDefinition(@"the user views the (home|search results) page")]
        public void OpenPage(string pageName)
        {
            _driverContext.GoToUri($"{_pageNameToUrlConverter[pageName]}");

            _searchPage.HeadingElement.Text.Should().Be("Search prototype");
        }

        [StepDefinition(@"the (.*) is accessible")]
        public void IsAccessible(string component)
        {
            // see https://github.com/dequelabs/axe-core/blob/develop/doc/API.md#axe-core-tags

            var outputFile = Path.Combine(
                _options.ArtifactsOutputPath,
                $"{_sessionOptions.Device}-axe-result-{component.ToLowerRemoveHyphens()}.json"
            );
            var axeResult = new AxeBuilder(_driverContext.Driver)
               .WithTags(_options.WcagTags)
               .WithOutputFile(outputFile)
               .Exclude(_searchPage.SearchHiddenDiv.Criteria)
               .Analyze();

            _logger.WriteLine($"Scan completed output location {outputFile}");

            // Check that axe ran successfuly https://github.com/dequelabs/axe-core/blob/develop/doc/API.md#error-result
            axeResult.Violations.Should().BeEmpty();

            var passCount = axeResult.Passes.Length;
            passCount.Should().NotBe(0);
            _logger.WriteLine($"Passed accessibility test count {passCount} for {component} at {axeResult.Url}");
        }
    }
}