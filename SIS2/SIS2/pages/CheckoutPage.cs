using Microsoft.Playwright;
using SIS2.data;

namespace SIS2.pages
{
    /// <summary>Steps 2-4 of /checkout: sign in, billing address, payment.</summary>
    public class CheckoutPage : BasePage
    {
        public CheckoutPage(IPage page) : base(page) { }

        // Step 2: sign in
        public ILocator Email => Page.GetByTestId("email");
        public ILocator Password => Page.GetByTestId("password");
        public ILocator LoginButton => Page.GetByTestId("login-submit");
        public ILocator ProceedFromSignIn => Page.GetByTestId("proceed-2");

        // Step 3: billing address
        public ILocator Country => Page.GetByTestId("country");
        public ILocator PostalCode => Page.GetByTestId("postal_code");
        public ILocator HouseNumber => Page.GetByTestId("house_number");
        public ILocator Street => Page.GetByTestId("street");
        public ILocator City => Page.GetByTestId("city");
        public ILocator State => Page.GetByTestId("state");
        public ILocator ProceedFromAddress => Page.GetByTestId("proceed-3");

        // Step 4: payment. The same button is first "Check payment", then "Confirm".
        public ILocator PaymentMethod => Page.GetByTestId("payment-method");
        public ILocator FinishButton => Page.GetByTestId("finish");
        public ILocator PaymentSuccess => Page.GetByTestId("payment-success-message");
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
