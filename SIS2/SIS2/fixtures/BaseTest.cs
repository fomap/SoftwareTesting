using Microsoft.Playwright;

namespace SIS2.fixtures
{
    /// <summary>
    /// Base for all Toolshop tests. PageTest already gives every test its own
    /// browser context and page, so tests share no cookies, storage or login state.
    /// </summary>
    public abstract class BaseTest : PageTest
    {
        public const string BaseUrl = "https://practicesoftwaretesting.com";

        [SetUp]
        public void UseDataTestAttribute()
        {
            // The app marks its elements with data-test="...", not data-testid.
            // This makes GetByTestId() target data-test attributes.
            Playwright.Selectors.SetTestIdAttribute("data-test");
        }
    }
}
