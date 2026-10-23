using Microsoft.Playwright;

namespace SIS2.pages
{
    public class RegisterPage
    {
        public readonly IPage _page;
        public RegisterPage(IPage page)
        {
            _page = page;
        }
    }
}
