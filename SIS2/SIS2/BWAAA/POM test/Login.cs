

using Microsoft.Playwright;



namespace SIS2
{
    public class Login : PageTest
    {
        [Test]
        public async Task LoginTest()
        {
            await Page.GotoAsync("https://commitquality.com/login");

            await Page.GetByPlaceholder("Enter Username").FillAsync("test");
            await Page.GetByPlaceholder("Enter Password").FillAsync("test");
            await Page.GetByTestId("login-button").ClickAsync();

            await Expect(Page.GetByText("Logout")).ToBeVisibleAsync();

        }

    }

}