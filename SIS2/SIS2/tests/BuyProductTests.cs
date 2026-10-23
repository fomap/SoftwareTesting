using SIS2.data;
using SIS2.fixtures;

namespace SIS2.tests
{
    /// <summary>Buy a product: add to cart -> cart shows the right item and quantity -> checkout completes.</summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class BuyProductTests : PagesFixture
    {
        [Test]
        public async Task AddToCart_CartShowsItemAndQuantity_CheckoutCompletes()
        {
            var customer = await CreateRegisteredCustomerAsync();
            var product = await ApiSetup.FirstInStockAsync(Playwright, Products.BuyCandidates);
            const int quantity = Products.QuantityToBuy;

            // Add to cart
            await ProductPage.SearchAndOpenAsync(product);
            await Expect(ProductPage.Name).ToHaveTextAsync(product);
            await Expect(ProductPage.OutOfStockLabel).ToBeHiddenAsync(); // nobody bought the last one meanwhile

            await ProductPage.SetQuantityAsync(quantity);
            await Expect(ProductPage.Quantity).ToHaveValueAsync(quantity.ToString());
            await ProductPage.AddToCartButton.ClickAsync();

            await Expect(ProductPage.AddedToCartAlert).ToBeVisibleAsync();
            await Expect(ProductPage.CartBadge).ToHaveTextAsync(quantity.ToString());

            // Cart shows the right item and quantity (and nothing else)
            await ProductPage.CartLink.ClickAsync();
            await Expect(CartPage.ItemNames).ToHaveCountAsync(1);
            await Expect(CartPage.ItemNames).ToHaveTextAsync(product);
            await Expect(CartPage.ItemQuantities).ToHaveValueAsync(quantity.ToString());

            // Checkout completes
            await CartPage.ProceedButton.ClickAsync();
            await CheckoutPage.SignInAsync(customer.Email, customer.Password);
            await CheckoutPage.FillAddressAsync(customer);
            await CheckoutPage.ProceedFromAddress.ClickAsync();

            await CheckoutPage.PaymentMethod.SelectOptionAsync(Products.PaymentMethod);
            await CheckoutPage.FinishButton.ClickAsync(); // "Check payment"
            await Expect(CheckoutPage.PaymentSuccess).ToHaveTextAsync("Payment was successful");

            // "Confirm" places the order. The app hides server errors here,
            // so check the server's answer to get a clear message if it is rejected.
            var order = await Page.RunAndWaitForResponseAsync(
                () => CheckoutPage.FinishButton.ClickAsync(),
                r => r.Url.EndsWith("/invoices") && r.Request.Method == "POST");
            Assert.That(order.Status, Is.EqualTo(201), $"Order rejected: {await order.TextAsync()}");

            await Expect(CheckoutPage.OrderConfirmation).ToHaveTextAsync(
                new Regex(@"Thanks for your order! Your invoice number is INV-\d+"));
        }
    }
}
