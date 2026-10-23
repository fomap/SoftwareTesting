using Microsoft.Playwright;

namespace SIS2.pages
{
    /// <summary>What every page of the shop shares: the browser page, the URL and the navbar.</summary>
    public abstract class BasePage
    {
        public const string BaseUrl = "https://practicesoftwaretesting.com";

        protected readonly IPage Page;

        protected BasePage(IPage page)
        {
            Page = page;
        }

        protected async Task OpenAsync(string path)
        {
            await Page.GotoAsync(BaseUrl + path);
        }

        public ILocator PageTitle => Page.GetByTestId("page-title");

        // Navbar
        public ILocator NavSignIn => Page.GetByTestId("nav-sign-in"); // only while signed out
        public ILocator UserMenu => Page.GetByTestId("nav-menu");     // shows the user's full name
        public ILocator CartBadge => Page.GetByTestId("cart-quantity");
        public ILocator CartLink => Page.GetByTestId("nav-cart");
    }
}
