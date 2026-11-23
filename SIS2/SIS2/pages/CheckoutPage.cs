using Microsoft.Playwright;
using SIS2.data;

namespace SIS2.pages
{
    /// <summary>Steps 2-4 of /checkout: sign in, billing address, payment.</summary>
    public class CheckoutPage : BasePage
    {
        public CheckoutPage(IPage page) : base(page) { }

        // Step 2: sign in
        public ILocator Email => TestId("email");
        public ILocator Password => TestId("password");
        public ILocator LoginButton => TestId("login-submit");
        public ILocator ProceedFromSignIn => TestId("proceed-2");

        // Step 3: billing address
        public ILocator Country => TestId("country");
        public ILocator PostalCode => TestId("postal_code");
        public ILocator HouseNumber => TestId("house_number");
        public ILocator Street => TestId("street");
        public ILocator City => TestId("city");
        public ILocator State => TestId("state");
        public ILocator ProceedFromAddress => TestId("proceed-3");

        // Step 4: payment. The same button is first "Check payment", then "Confirm".
        public ILocator PaymentMethod => TestId("payment-method");
        public ILocator FinishButton => TestId("finish");
        public ILocator PaymentSuccess => TestId("payment-success-message");
        public ILocator OrderConfirmation => Page.GetByText(new Regex("Thanks for your order!"));

        public async Task SignInAsync(string email, string password)
        {
            await Email.FillAsync(email);
            await Password.FillAsync(password);
            await LoginButton.ClickAsync();
            await ProceedFromSignIn.ClickAsync();
        }

        public async Task FillAddressAsync(Customer c)
        {
            await Country.SelectOptionAsync(c.Country);
            await PostalCode.FillAsync(c.PostalCode);
            await HouseNumber.FillAsync(c.HouseNumber);
            await Street.FillAsync(c.Street);
            await City.FillAsync(c.City);
            await State.FillAsync(c.State);
        }
    }
}
