using Microsoft.Playwright;

namespace SIS2.pages
{
    public class BuyProductPage
    {
        public readonly IPage _page;
        public BuyProductPage(IPage page)
        {
            _page = page;
        }
    }
}
