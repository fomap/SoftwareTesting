using Microsoft.Playwright;
using SIS2.data;

namespace SIS2.fixtures
{
    /// <summary>
    /// Base for all Toolshop tests. PageTest already gives every test its own
    /// browser context and page, so tests share no cookies, storage, cart or login state.
    /// </summary>
    public abstract class BaseTest : PageTest
    {
        public const string BaseUrl = "https://practicesoftwaretesting.com";

        /// <summary>
        /// The site is public and shared, so the server can be slow. Assertions that wait
        /// for a server answer get this much room. It is a maximum, not a fixed wait:
        /// the assertion passes as soon as the condition is true.
        /// </summary>
        public const float SlowServerTimeout = 15_000;

        [SetUp]
        public void UseDataTestAttribute()
        {
            // The app marks its elements with data-test="...", not data-testid.
            // This makes GetByTestId() target data-test attributes.
            Playwright.Selectors.SetTestIdAttribute("data-test");
        }

        /// <summary>
        /// A fresh customer that already exists on the site (registered through the API).
        /// Used instead of the shared seeded accounts, which other people's failed
        /// logins can lock, and which a password reset would change for everyone.
        /// </summary>
        protected async Task<CustomerData> CreateRegisteredCustomerAsync()
        {
            var customer = UserFactory.NewCustomer();
            await AccountApi.RegisterAsync(Playwright, customer);
            return customer;
        }

        /// <summary>Regex that matches a URL ending with the given path.</summary>
        protected static Regex UrlEndsWith(string path) => new(Regex.Escape(path) + "$");
    }
}
