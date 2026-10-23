using Microsoft.Playwright;

namespace SIS2.pages
{
    public class ResetPasswordPage
    {
        public readonly IPage _page;
        public ResetPasswordPage(IPage page)
        {
            _page = page;
        }
    }
}
