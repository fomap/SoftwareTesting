using Microsoft.Playwright;
using SIS2.data;

namespace SIS2.pages
{
    /// <summary>A single product's detail page.</summary>
    public class ProductPage
    {
        public readonly IPage _page;

        public ProductPage(IPage page)
        {
            _page = page;
        }

        public ILocator Name => _page.GetByTestId("product-name");
        public ILocator UnitPrice => _page.GetByTestId("unit-price");
        /// <summary>Quantity box. It is disabled for typing; use the +/- buttons.</summary>
        public ILocator Quantity => _page.GetByTestId("quantity");
        public ILocator IncreaseQuantityButton => _page.GetByTestId("increase-quantity");
        public ILocator AddToCartButton => _page.GetByTestId("add-to-cart");

        /// <summary>"Out of stock" label. When it shows, quantity and "Add to cart" are disabled.</summary>
        public ILocator OutOfStockLabel => _page.GetByTestId("out-of-stock");

        /// <summary>Confirmation that pops up after "Add to cart".</summary>
        public ILocator AddedToCartAlert =>
            _page.GetByRole(AriaRole.Alert).Filter(new() { HasTextString = "Product added to shopping cart" });

        /// <summary>Number badge on the cart icon in the navbar.</summary>
        public ILocator CartBadge => _page.GetByTestId("cart-quantity");
        public ILocator CartLink => _page.GetByTestId("nav-cart");

        /// <summary>
        /// Raises the quantity from its starting value of 1 by clicking "+".
        /// The quantity box itself is disabled, so it cannot be typed into.
        /// The test asserts the resulting value.
        /// </summary>
        public async Task SetQuantityAsync(int quantity)
        {
            for (var current = 1; current < quantity; current++)
            {
                await IncreaseQuantityButton.ClickAsync();
            }
        }

        public async Task AddToCartAsync()
        {
            await AddToCartButton.ClickAsync();
        }

        /// <summary>
        /// Reads name and price. Call it after asserting the page shows the expected
        /// product, so the values come from a fully loaded page.
        /// </summary>
        public async Task<ProductSnapshot> CaptureAsync() =>
            new((await Name.InnerTextAsync()).Trim(), Prices.Parse(await UnitPrice.InnerTextAsync()));
    }
}
