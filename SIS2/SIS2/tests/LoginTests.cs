using SIS2.data;
using SIS2.fixtures;

namespace SIS2.tests
{
    /// <summary>Log in: valid credentials reach the account page; invalid ones show the error and do not.</summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class LoginTests : PagesFixture
    {
        [Test]
        public async Task ValidCredentials_ReachAccountPage()
        {
            var customer = await CreateRegisteredCustomerAsync();

            await LoginPage.OpenAsync();
            await LoginPage.LoginAsync(customer.Email, customer.Password);

            await Expect(Page).ToHaveURLAsync(new Regex("/account$"));
            await Expect(LoginPage.PageTitle).ToHaveTextAsync("My account");
            await Expect(LoginPage.UserMenu).ToContainTextAsync(customer.FullName);
        }

        [Test]
        public async Task WrongPassword_ShowsError_AndDoesNotSignIn()
        {
            var customer = await CreateRegisteredCustomerAsync();

            await LoginPage.OpenAsync();
            await LoginPage.LoginAsync(customer.Email, UserFactory.WrongPassword());

            await Expect(LoginPage.LoginError).ToHaveTextAsync("Invalid email or password");
            await Expect(Page).ToHaveURLAsync(new Regex("/auth/login$"));
            await Expect(LoginPage.NavSignIn).ToBeVisibleAsync(); // still signed out
            await Expect(LoginPage.UserMenu).ToBeHiddenAsync();
        }
    }
}
