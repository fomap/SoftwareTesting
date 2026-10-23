using Microsoft.Playwright;
using SIS2.data;
using SIS2.fixtures;

namespace SIS2.pages
{
    public class RegisterPage
    {
        public readonly IPage _page;

        public RegisterPage(IPage page)
        {
            _page = page;
        }

        public const string Path = "/auth/register";

        public ILocator FirstName => _page.GetByTestId("first-name");
        public ILocator LastName => _page.GetByTestId("last-name");
        public ILocator DateOfBirth => _page.GetByTestId("dob");
        public ILocator Country => _page.GetByTestId("country");
        public ILocator PostalCode => _page.GetByTestId("postal_code");
        public ILocator HouseNumber => _page.GetByTestId("house_number");
        public ILocator Street => _page.GetByTestId("street");
        public ILocator City => _page.GetByTestId("city");
        public ILocator State => _page.GetByTestId("state");
        public ILocator Phone => _page.GetByTestId("phone");
        public ILocator Email => _page.GetByTestId("email");
        public ILocator Password => _page.GetByTestId("password");
        public ILocator SubmitButton => _page.GetByTestId("register-submit");

        /// <summary>Server-side error shown under the form, e.g. "email already exists".</summary>
        public ILocator RegisterError => _page.GetByTestId("register-error");

        public async Task GotoAsync()
        {
            await _page.GotoAsync(BaseTest.BaseUrl + Path);
        }

        /// <summary>Fills every field with the customer's data. Does not submit.</summary>
        public async Task FillFormAsync(CustomerData c)
        {
            await FirstName.FillAsync(c.FirstName);
            await LastName.FillAsync(c.LastName);
            await DateOfBirth.FillAsync(c.DateOfBirth);
            await Country.SelectOptionAsync(c.CountryCode);
            await PostalCode.FillAsync(c.PostalCode);
            await HouseNumber.FillAsync(c.HouseNumber);
            // Country + postcode + house number trigger an address lookup that may
            // fill these three in the background. We fill them anyway, so the form is
            // valid whether or not the lookup answers (or overwrites them later).
            await Street.FillAsync(c.Street);
            await City.FillAsync(c.City);
            await State.FillAsync(c.State);
            await Phone.FillAsync(c.Phone);
            await Email.FillAsync(c.Email);
            await Password.FillAsync(c.Password);
        }

        public async Task SubmitAsync()
        {
            await SubmitButton.ClickAsync();
        }

        public async Task RegisterAsync(CustomerData c)
        {
            await FillFormAsync(c);
            await SubmitAsync();
        }
    }
}
