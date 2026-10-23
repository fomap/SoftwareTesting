using Microsoft.Playwright;
using SIS2.data;
using SIS2.fixtures;

namespace SIS2.pages
{
    /// <summary>Product listing with search (the site's home page).</summary>
    public class HomePage
    {
        public readonly IPage _page;

        public HomePage(IPage page)
        {
            _page = page;
        }

        public ILocator SearchInput => _page.GetByTestId("search-query");
        public ILocator SearchButton => _page.GetByTestId("search-submit");
        public ILocator SearchTerm => _page.GetByTestId("search-term");

        public async Task GotoAsync()
        {
            await _page.GotoAsync(BaseTest.BaseUrl + "/");
        }

        /// <summary>Name on the product card whose name is exactly <paramref name="name"/>.</summary>
        public ILocator ProductName(string name) =>
            _page.GetByTestId("product-name").Filter(new() { HasTextRegex = ExactText(name) });

        /// <summary>The whole product card (a link) for that product.</summary>
        public ILocator ProductCard(string name) =>
            _page.GetByRole(AriaRole.Link).Filter(new() { Has = ProductName(name) });

        /// <summary>Price shown on that product's card, e.g. "$48.41".</summary>
        public ILocator ProductCardPrice(string name) =>
            ProductCard(name).GetByTestId("product-price");

        /// <summary>
        /// Searches and waits until the results for this term are shown.
        /// (Before that, the default listing is still on screen.)
        /// </summary>
        public async Task SearchAsync(string term)
        {
            await SearchInput.FillAsync(term);
            await SearchButton.ClickAsync();
            await SearchTerm.Filter(new() { HasTextRegex = ExactText(term) })
                .WaitForAsync(new() { Timeout = BaseTest.SlowServerTimeout });
        }

        /// <summary>Price on the listing card, as a number.</summary>
        public async Task<decimal> GetCardPriceAsync(string name) =>
            Prices.Parse(await ProductCardPrice(name).InnerTextAsync());

        public async Task OpenProductAsync(string name)
        {
            await ProductName(name).ClickAsync();
        }

        private static Regex ExactText(string text) => new("^\\s*" + Regex.Escape(text) + "\\s*$");
    }
}
