using Microsoft.Playwright;

namespace SIS2.pages
{
    /// <summary>Step 1 of /checkout: the cart.</summary>
    public class CartPage : BasePage
    {
        public CartPage(IPage page) : base(page) { }

        public ILocator ItemNames => TestId("product-title");
        public ILocator ItemQuantities => TestId("product-quantity");
        public ILocator ProceedButton => TestId("proceed-1");
    }
}
