using Microsoft.Playwright;

namespace SIS2.pages
{
    /// <summary>Login page and the "Forgot your password?" page reached from it.</summary>
    public class LoginPage : BasePage
    {
        public LoginPage(IPage page) : base(page) { }

        public ILocator Email => Page.GetByTestId("email");
        public ILocator Password => Page.GetByTestId("password");
        public ILocator LoginButton => Page.GetByTestId("login-submit");
        public ILocator LoginError => Page.GetByTestId("login-error");

        // Forgot password
        public ILocator ResetButton => Page.GetByTestId("forgot-password-submit");

        /// <summary>
        /// Result of a reset request. It only has role="alert" (no data-test)
        /// and disappears after about 3 seconds.
        /// </summary>
        public ILocator ResetResult => Page.GetByRole(AriaRole.Alert);

        public Task OpenAsync() => OpenAsync("/auth/login");

        public Task OpenForgotPasswordAsync() => OpenAsync("/auth/forgot-password");

        public async Task LoginAsync(string email, string password)
        {
            await Email.FillAsync(email);
            await Password.FillAsync(password);
            await LoginButton.ClickAsync();
        }

        public async Task RequestPasswordResetAsync(string email)
        {
            await Email.FillAsync(email);
            await ResetButton.ClickAsync();
        }
    }
}
