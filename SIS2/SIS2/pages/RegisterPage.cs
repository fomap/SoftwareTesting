using Microsoft.Playwright;
using SIS2.data;

namespace SIS2.pages
{
    public class RegisterPage : BasePage
    {
        public RegisterPage(IPage page) : base(page) { }

        public ILocator FirstName => TestId("first-name");
        public ILocator LastName => TestId("last-name");
        public ILocator DateOfBirth => TestId("dob");
        public ILocator Country => TestId("country");
        public ILocator PostalCode => TestId("postal_code");
        public ILocator HouseNumber => TestId("house_number");
        public ILocator Street => TestId("street");
        public ILocator City => TestId("city");
        public ILocator State => TestId("state");
        public ILocator Phone => TestId("phone");
        public ILocator Email => TestId("email");
        public ILocator Password => TestId("password");
        public ILocator SubmitButton => TestId("register-submit");

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
