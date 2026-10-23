using Microsoft.Playwright;

namespace SIS2.pages
{
    public class CompareProductsPage
    {
        public readonly IPage _page;
        public CompareProductsPage(IPage page)
        {
            _page = page;
        }
    }
}
