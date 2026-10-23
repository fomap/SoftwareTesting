using Microsoft.Playwright;
using SIS2.data;
using SIS2.fixtures;
using SIS2.pages;

namespace SIS2.tests
{
    /// <summary>
    /// Test case 5: Reset a password.
    /// Must prove: the forgot-password flow gives the expected confirmation for a known
    /// email, and an error for an invalid one.
    ///
    /// On this site a reset really changes the account's password. Running it on a
    /// shared seeded account would change the password for everyone using it, so the
    /// "known email" is an account this test registers for itself.
    ///
    /// The result message disappears about 3 seconds after it appears. Expect() checks
    /// it as soon as it shows up, so no extra waiting is needed.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class ResetPasswordTests : BaseTest
    {
        private const string UnknownEmailError = "The selected email is invalid.";

        private ForgotPasswordPage _forgotPage = null!;

        [SetUp]
        public void CreatePages()
        {
            _forgotPage = new ForgotPasswordPage(Page);
        }

        [Test]
        public async Task KnownEmail_ShowsConfirmation()
        {
            var customer = await CreateRegisteredCustomerAsync();

            await _forgotPage.GotoAsync();
            await _forgotPage.RequestResetAsync(customer.Email);

            // The site currently renders its confirmation as the raw translation key
            // "page.forgot-password.confirm" (an app bug: missing translation). We assert
            // it is a confirmation, not the error, without pinning the broken wording.
            await Expect(_forgotPage.ResultAlert).ToContainTextAsync(
                new Regex("confirm|success|sent|updated", RegexOptions.IgnoreCase),
                new() { Timeout = SlowServerTimeout });
            await Expect(_forgotPage.ResultAlert).Not.ToContainTextAsync(UnknownEmailError);
            await Expect(Page).ToHaveURLAsync(UrlEndsWith(ForgotPasswordPage.Path));
        }

        [Test]
        public async Task UnknownEmail_ShowsError()
        {
            await _forgotPage.GotoAsync();
            await _forgotPage.RequestResetAsync(UserFactory.UnregisteredEmail());

            await Expect(_forgotPage.ResultAlert).ToHaveTextAsync(UnknownEmailError,
                new() { Timeout = SlowServerTimeout });
            await Expect(Page).ToHaveURLAsync(UrlEndsWith(ForgotPasswordPage.Path));
        }
    }
}
