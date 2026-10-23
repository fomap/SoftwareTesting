using Microsoft.Playwright;

namespace SIS2.pages
{
    public class AccountPage
    {
        public readonly IPage _page;

        public AccountPage(IPage page)
        {
            _page = page;
        }

        public const string Path = "/account";

        public ILocator PageTitle => _page.GetByTestId("page-title");

        /// <summary>The signed-in user's menu in the navbar; its text is the user's full name.</summary>
        public ILocator UserMenu => _page.GetByTestId("nav-menu");
    }
}
