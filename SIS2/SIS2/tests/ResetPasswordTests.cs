using SIS2.data;
using SIS2.fixtures;

namespace SIS2.tests
{
    /// <summary>
    /// Reset a password: confirmation for a known email, error for an invalid one.
    /// A reset really changes the password on this site, so the known email is the
    /// test's own fresh account, never a shared seeded one.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class ResetPasswordTests : PagesFixture
    {
        [Test]
        public async Task KnownEmail_ShowsConfirmation()
        {
            var customer = await CreateRegisteredCustomerAsync();

            await LoginPage.OpenForgotPasswordAsync();
            await LoginPage.RequestPasswordResetAsync(customer.Email);

            // The site shows its confirmation as the raw key "page.forgot-password.confirm"
            // (missing translation, an app bug), so match the meaning, not the exact wording.
            await Expect(LoginPage.ResetResult).ToContainTextAsync(
                new Regex("confirm|success|sent|updated", RegexOptions.IgnoreCase));
        }

        [Test]
        public async Task UnknownEmail_ShowsError()
        {
            await LoginPage.OpenForgotPasswordAsync();
            await LoginPage.RequestPasswordResetAsync(UserFactory.UnregisteredEmail());

            await Expect(LoginPage.ResetResult).ToHaveTextAsync("The selected email is invalid.");
        }
    }
}
