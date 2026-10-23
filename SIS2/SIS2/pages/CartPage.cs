using Microsoft.Playwright;

namespace SIS2.pages
{
    /// <summary>Step 1 of /checkout: the cart.</summary>
    public class CartPage : BasePage
    {
        public CartPage(IPage page) : base(page) { }

        public ILocator ItemNames => Page.GetByTestId("product-title");
        public ILocator ItemQuantities => Page.GetByTestId("product-quantity");
        public ILocator ProceedButton => Page.GetByTestId("proceed-1");
    }
}
