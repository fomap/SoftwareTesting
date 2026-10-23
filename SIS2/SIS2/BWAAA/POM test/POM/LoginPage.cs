using Microsoft.Playwright;

namespace SIS2.POM
{
    public class LoginPage
    {
        public readonly IPage _page;
        public LoginPage(IPage page)
        {
            _page = page;
        }

        private ILocator Username => _page.GetByPlaceholder("Enter Username");
        private ILocator Password => _page.GetByPlaceholder("Enter Password");
        private ILocator LoginButton => _page.GetByTestId("login-button");


        public async Task Login(string username, string password)
        {
            await Username.FillAsync(username);
            await Password.FillAsync(password);
            await LoginButton.ClickAsync();

        }

        public async Task GoTo()
        {
            await _page.GotoAsync("https://commitquality.com/login");

        }



    }
}
