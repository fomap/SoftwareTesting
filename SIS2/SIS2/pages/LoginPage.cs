using Microsoft.Playwright;

namespace SIS2.pages
{
    public class LoginPage
    {
        public readonly IPage _page;
        public LoginPage(IPage page)
        {
            _page = page;
        }
    }
}
