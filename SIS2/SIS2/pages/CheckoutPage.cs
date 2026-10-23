using Microsoft.Playwright;
using SIS2.data;
using SIS2.fixtures;

namespace SIS2.pages
{
    /// <summary>
    /// /checkout: a 4-step wizard. 1 Cart, 2 Sign in, 3 Billing address, 4 Payment.
    /// </summary>
    public class CheckoutPage
    {
        public readonly IPage _page;

        public CheckoutPage(IPage page)
        {
            _page = page;
        }

        public const string Path = "/checkout";

        // ---- Step 1: cart ----

        /// <summary>The cart table row for this product.</summary>
        public ILocator CartRow(string productName) =>
            _page.GetByRole(AriaRole.Row).Filter(new()
            {
                Has = _page.GetByTestId("product-title").Filter(new() { HasTextString = productName }),
            });

        public ILocator RowTitle(string productName) => CartRow(productName).GetByTestId("product-title");
        public ILocator RowQuantity(string productName) => CartRow(productName).GetByTestId("product-quantity");
        public ILocator RowUnitPrice(string productName) => CartRow(productName).GetByTestId("product-price");
        public ILocator RowLinePrice(string productName) => CartRow(productName).GetByTestId("line-price");
        public ILocator CartTotal => _page.GetByTestId("cart-total");
        public ILocator ProceedFromCart => _page.GetByTestId("proceed-1");

        // ---- Step 2: sign in ----
        public ILocator Email => _page.GetByTestId("email");
        public ILocator Password => _page.GetByTestId("password");
        public ILocator LoginButton => _page.GetByTestId("login-submit");
        public ILocator ProceedFromSignIn => _page.GetByTestId("proceed-2");

        // ---- Step 3: billing address ----
        public ILocator Country => _page.GetByTestId("country");
        public ILocator PostalCode => _page.GetByTestId("postal_code");
        public ILocator HouseNumber => _page.GetByTestId("house_number");
        public ILocator Street => _page.GetByTestId("street");
        public ILocator City => _page.GetByTestId("city");
        public ILocator State => _page.GetByTestId("state");
        public ILocator ProceedFromAddress => _page.GetByTestId("proceed-3");

        // ---- Step 4: payment ----
        public ILocator PaymentMethod => _page.GetByTestId("payment-method");

        /// <summary>Same button twice: first "Check payment", then "Confirm".</summary>
        public ILocator FinishButton => _page.GetByTestId("finish");
        public ILocator PaymentSuccessMessage => _page.GetByTestId("payment-success-message");

        /// <summary>"Thanks for your order! Your invoice number is INV-...".</summary>
        public ILocator OrderConfirmation => _page.GetByText(new Regex("Thanks for your order!"));

        public async Task GotoAsync()
        {
            await _page.GotoAsync(BaseTest.BaseUrl + Path);
        }

        public async Task SignInAsync(string email, string password)
        {
            await Email.FillAsync(email);
            await Password.FillAsync(password);
            await LoginButton.ClickAsync();
        }

        public async Task FillBillingAddressAsync(CustomerData c)
        {
            // The form may be pre-filled from the account, and the postcode lookup may
            // fill street/city/state in the background. Filling every field makes the
            // result the same either way.
            await Country.SelectOptionAsync(c.CountryCode);
            await PostalCode.FillAsync(c.PostalCode);
            await HouseNumber.FillAsync(c.HouseNumber);
            await Street.FillAsync(c.Street);
            await City.FillAsync(c.City);
            await State.FillAsync(c.State);
        }

        public async Task ChoosePaymentAsync(string paymentMethodValue)
        {
            await PaymentMethod.SelectOptionAsync(paymentMethodValue);
        }
    }
}
