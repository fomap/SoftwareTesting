using Microsoft.Playwright;
using SIS2.fixtures;

namespace SIS2.pages
{
    public class ForgotPasswordPage
    {
        public readonly IPage _page;

        public ForgotPasswordPage(IPage page)
        {
            _page = page;
        }

        public const string Path = "/auth/forgot-password";

        public ILocator Email => _page.GetByTestId("email");
        public ILocator SubmitButton => _page.GetByTestId("forgot-password-submit");

        /// <summary>
        /// The result message (success or error). It has no data-test attribute, only
        /// role="alert", and the app hides it again after about 3 seconds, so assert on
        /// it right after submitting.
        /// </summary>
        public ILocator ResultAlert => _page.GetByRole(AriaRole.Alert);

        public async Task GotoAsync()
        {
            await _page.GotoAsync(BaseTest.BaseUrl + Path);
        }

        public async Task RequestResetAsync(string email)
        {
            await Email.FillAsync(email);
            await SubmitButton.ClickAsync();
        }
    }
}
