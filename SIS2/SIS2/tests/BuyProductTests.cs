using Microsoft.Playwright;
using SIS2.data;
using SIS2.fixtures;
using SIS2.pages;

namespace SIS2.tests
{
    /// <summary>
    /// Test case 3: Buy a product.
    /// Must prove: add to cart -> cart shows the right item and quantity -> checkout completes.
    ///
    /// The cart belongs to this test's own browser context, and the buyer is this
    /// test's own freshly registered customer, so parallel runs and other shoppers
    /// on the site cannot change what this test sees.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class BuyProductTests : BaseTest
    {
        private HomePage _homePage = null!;
        private ProductPage _productPage = null!;
        private CheckoutPage _checkout = null!;

        [SetUp]
        public void CreatePages()
        {
            _homePage = new HomePage(Page);
            _productPage = new ProductPage(Page);
            _checkout = new CheckoutPage(Page);
        }

        [Test]
        public async Task AddToCart_CartShowsItemAndQuantity_CheckoutCompletes()
        {
            var customer = await CreateRegisteredCustomerAsync();
            const string product = Products.ToBuy;
            const int quantity = Products.QuantityToBuy;

            // ---- Add to cart ----
            await _homePage.GotoAsync();
            await _homePage.SearchAsync(product);
            await _homePage.OpenProductAsync(product);

            await Expect(_productPage.Name).ToHaveTextAsync(product);
            var unitPrice = (await _productPage.CaptureAsync()).Price;

            await Expect(_productPage.Quantity).ToHaveValueAsync("1");   // starting point
            await _productPage.SetQuantityAsync(quantity);
            await Expect(_productPage.Quantity).ToHaveValueAsync(quantity.ToString());
            await _productPage.AddToCartAsync();

            await Expect(_productPage.AddedToCartAlert).ToBeVisibleAsync(new() { Timeout = SlowServerTimeout });
            await Expect(_productPage.CartBadge).ToHaveTextAsync(quantity.ToString());

            // ---- Cart shows the right item and quantity ----
            await _productPage.CartLink.ClickAsync();
            await Expect(Page).ToHaveURLAsync(UrlEndsWith(CheckoutPage.Path));

            await Expect(_checkout.RowTitle(product)).ToHaveTextAsync(product);
            await Expect(_checkout.RowQuantity(product)).ToHaveValueAsync(quantity.ToString());
            await Expect(_checkout.RowUnitPrice(product)).ToHaveTextAsync(Prices.Format(unitPrice));
            await Expect(_checkout.RowLinePrice(product)).ToHaveTextAsync(Prices.Format(unitPrice * quantity));
            await Expect(_checkout.CartTotal).ToHaveTextAsync(Prices.Format(unitPrice * quantity));

            // ---- Checkout completes ----
            await _checkout.ProceedFromCart.ClickAsync();

            await _checkout.SignInAsync(customer.Email, customer.Password);
            await _checkout.ProceedFromSignIn.ClickAsync(new() { Timeout = SlowServerTimeout });

            await _checkout.FillBillingAddressAsync(customer);
            await Expect(_checkout.ProceedFromAddress).ToBeEnabledAsync();
            await _checkout.ProceedFromAddress.ClickAsync();

            await _checkout.ChoosePaymentAsync(PaymentMethods.CashOnDelivery);
            await _checkout.FinishButton.ClickAsync();                  // "Check payment"
            await Expect(_checkout.PaymentSuccessMessage).ToHaveTextAsync("Payment was successful",
                new() { Timeout = SlowServerTimeout });

            await _checkout.FinishButton.ClickAsync();                  // "Confirm"
            await Expect(_checkout.OrderConfirmation).ToHaveTextAsync(
                new Regex(@"Thanks for your order! Your invoice number is INV-\d+"),
                new() { Timeout = SlowServerTimeout });
        }
    }
}
