using Microsoft.Playwright;
using SIS2.fixtures;

namespace SIS2.pages
{
    public class LoginPage
    {
        public readonly IPage _page;

        public LoginPage(IPage page)
        {
            _page = page;
        }

        public const string Path = "/auth/login";

        public ILocator Email => _page.GetByTestId("email");
        public ILocator Password => _page.GetByTestId("password");
        public ILocator SubmitButton => _page.GetByTestId("login-submit");
        public ILocator LoginError => _page.GetByTestId("login-error");

        public async Task GotoAsync()
        {
            await _page.GotoAsync(BaseTest.BaseUrl + Path);
        }

        public async Task LoginAsync(string email, string password)
        {
            await Email.FillAsync(email);
            await Password.FillAsync(password);
            await SubmitButton.ClickAsync();
        }
    }
}
