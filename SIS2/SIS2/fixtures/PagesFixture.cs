using SIS2.data;
using SIS2.pages;

namespace SIS2.fixtures
{
    /// <summary>
    /// Base class for every test. Playwright's PageTest opens a fresh browser context
    /// (own cookies, storage, cart, login) per test; this fixture adds ready-made page
    /// objects and a way to get a customer that already exists on the site.
    /// </summary>
    public abstract class PagesFixture : PageTest
    {
        protected RegisterPage RegisterPage = null!;
        protected LoginPage LoginPage = null!;
        protected ProductPage ProductPage = null!;
        protected CartPage CartPage = null!;
        protected CheckoutPage CheckoutPage = null!;

        [SetUp]
        public void CreatePages()
        {
            // The app marks elements with data-test="...", so GetByTestId() should use that.
            Playwright.Selectors.SetTestIdAttribute("data-test");

            RegisterPage = new RegisterPage(Page);
            LoginPage = new LoginPage(Page);
            ProductPage = new ProductPage(Page);
            CartPage = new CartPage(Page);
            CheckoutPage = new CheckoutPage(Page);
        }

        /// <summary>A brand-new customer, registered through the API so the test can log in straight away.</summary>
        protected Task<Customer> CreateRegisteredCustomerAsync() =>
            ApiSetup.RegisterNewCustomerAsync(Playwright);
    }
}
