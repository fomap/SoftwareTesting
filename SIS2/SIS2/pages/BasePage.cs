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
