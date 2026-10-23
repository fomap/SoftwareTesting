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
            // Stock on the shared site changes all the time, so pick a product that is in stock now.
            var product = await ProductApi.FirstInStockAsync(Playwright, Products.BuyCandidates);
            const int quantity = Products.QuantityToBuy;
            NUnit.Framework.TestContext.Progress.WriteLine($"Buying {quantity} x {product}");

            // ---- Add to cart ----
            await _homePage.GotoAsync();
            await _homePage.SearchAsync(product);
            await _homePage.OpenProductAsync(product);

            await Expect(_productPage.Name).ToHaveTextAsync(product);
            // The UI must agree it can be bought (someone may have bought the last one meanwhile).
            await Expect(_productPage.OutOfStockLabel).ToBeHiddenAsync();
            await Expect(_productPage.AddToCartButton).ToBeEnabledAsync();
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

            // "Confirm" creates the order. If the server rejects it, the app shows nothing,
            // so we check the server's answer directly to get a clear failure message.
            var invoiceResponse = await Page.RunAndWaitForResponseAsync(
                () => _checkout.FinishButton.ClickAsync(),
                r => r.Url.EndsWith("/invoices") && r.Request.Method == "POST",
                new() { Timeout = SlowServerTimeout });
            Assert.That(invoiceResponse.Status, Is.EqualTo(201),
                $"Order was rejected by the server: {await invoiceResponse.TextAsync()}");

            await Expect(_checkout.OrderConfirmation).ToHaveTextAsync(
                new Regex(@"Thanks for your order! Your invoice number is INV-\d+"),
                new() { Timeout = SlowServerTimeout });
        }
    }
}
