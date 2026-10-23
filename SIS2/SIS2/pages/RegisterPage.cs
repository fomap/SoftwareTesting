using Microsoft.Playwright;
using SIS2.data;

namespace SIS2.pages
{
    public class RegisterPage : BasePage
    {
        public RegisterPage(IPage page) : base(page) { }

        public ILocator FirstName => Page.GetByTestId("first-name");
        public ILocator LastName => Page.GetByTestId("last-name");
        public ILocator DateOfBirth => Page.GetByTestId("dob");
        public ILocator Country => Page.GetByTestId("country");
        public ILocator PostalCode => Page.GetByTestId("postal_code");
        public ILocator HouseNumber => Page.GetByTestId("house_number");
        public ILocator Street => Page.GetByTestId("street");
        public ILocator City => Page.GetByTestId("city");
        public ILocator State => Page.GetByTestId("state");
        public ILocator Phone => Page.GetByTestId("phone");
        public ILocator Email => Page.GetByTestId("email");
        public ILocator Password => Page.GetByTestId("password");
        public ILocator SubmitButton => Page.GetByTestId("register-submit");

        public Task OpenAsync() => OpenAsync("/auth/register");

        public async Task RegisterAsync(Customer c)
        {
            await FirstName.FillAsync(c.FirstName);
            await LastName.FillAsync(c.LastName);
            await DateOfBirth.FillAsync(c.DateOfBirth);
            await Country.SelectOptionAsync(c.Country);
            await PostalCode.FillAsync(c.PostalCode);
            await HouseNumber.FillAsync(c.HouseNumber);
            // The postcode lookup may also fill these in the background; either way they are valid.
            await Street.FillAsync(c.Street);
            await City.FillAsync(c.City);
            await State.FillAsync(c.State);
            await Phone.FillAsync(c.Phone);
            await Email.FillAsync(c.Email);
            await Password.FillAsync(c.Password);
            await SubmitButton.ClickAsync();
        }
    }
}
