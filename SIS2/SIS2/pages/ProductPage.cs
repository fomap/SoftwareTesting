using Microsoft.Playwright;
using System.Globalization;

namespace SIS2.pages
{
    /// <summary>Finding a product (search on the shop's home page) and its detail page.</summary>
    public class ProductPage : BasePage
    {
        public ProductPage(IPage page) : base(page) { }

        // Search on the home page
        public ILocator SearchInput => TestId("search-query");
        public ILocator SearchButton => TestId("search-submit");
        public ILocator SearchTerm => TestId("search-term");

        // Product detail
        public ILocator Name => TestId("product-name");
        public ILocator UnitPrice => TestId("unit-price");
        public ILocator Quantity => TestId("quantity");
        public ILocator IncreaseQuantityButton => TestId("increase-quantity");
        public ILocator AddToCartButton => TestId("add-to-cart");
        public ILocator OutOfStockLabel => TestId("out-of-stock");
        public ILocator AddedToCartAlert =>
            Page.GetByRole(AriaRole.Alert).Filter(new() { HasTextString = "Product added to shopping cart" });

        /// <summary>
        /// Searches for the product and opens it. Products are found by name, not by
        /// URL, because product ids change when the demo database is reset.
        /// </summary>
        public async Task SearchAndOpenAsync(string name)
        {
            await OpenAsync("/");
            await SearchInput.FillAsync(name);
            await SearchButton.ClickAsync();
            await SearchTerm.Filter(new() { HasTextString = name }).WaitForAsync(); // results are in

            // Exact name: a search for "Pliers" also lists "Slip Joint Pliers".
            var exactName = new Regex("^\\s*" + Regex.Escape(name) + "\\s*$");
            await TestId("product-name").Filter(new() { HasTextRegex = exactName }).ClickAsync();
        }

        public Task OpenByIdAsync(string id) => OpenAsync($"/product/{id}");

        /// <summary>Quantity starts at 1 and the box can't be typed into, so click "+".</summary>
        public async Task SetQuantityAsync(int quantity)
        {
            for (var current = 1; current < quantity; current++)
            {
                await IncreaseQuantityButton.ClickAsync();
            }
        }

        public async Task<(string Name, decimal Price)> ReadNameAndPriceAsync()
        {
            var name = (await Name.InnerTextAsync()).Trim();
            var price = decimal.Parse((await UnitPrice.InnerTextAsync()).Replace("$", "").Trim(),
                CultureInfo.InvariantCulture);
            return (name, price);
        }
    }
}
