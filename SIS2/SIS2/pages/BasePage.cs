using Microsoft.Playwright;

namespace SIS2.pages
{

    public abstract class BasePage
    {
        public static readonly string BaseUrl =
            Environment.GetEnvironmentVariable("BASE_URL") ?? "https://practicesoftwaretesting.com";

        protected readonly IPage Page;

        protected BasePage(IPage page)
        {
            Page = page;
        }

        protected ILocator TestId(string id) => Page.Locator($"[data-test=\"{id}\"]");

        protected async Task OpenAsync(string path)
        {
            await Page.GotoAsync(BaseUrl + path);
        }

        public ILocator PageTitle => TestId("page-title");

        // Navbar
        public ILocator NavSignIn => TestId("nav-sign-in"); // only while signed out
        public ILocator UserMenu => TestId("nav-menu");     // shows the user's full name
        public ILocator CartBadge => TestId("cart-quantity");
        public ILocator CartLink => TestId("nav-cart");
    }
}
