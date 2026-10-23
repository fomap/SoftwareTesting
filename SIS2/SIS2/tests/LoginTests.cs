using Microsoft.Playwright;
using SIS2.data;
using SIS2.fixtures;
using SIS2.pages;

namespace SIS2.tests
{
    /// <summary>
    /// Test case 2: Log in.
    /// Must prove: valid credentials reach the account page; invalid ones show the error and do not.
    ///
    /// Each test registers its own customer (via API) first. The seeded accounts are
    /// shared with everyone using the site, and other people's failed logins lock them.
    /// Typing a wrong password into a seeded account would also add to that lockout,
    /// so the "wrong password" test uses its own account too.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class LoginTests : BaseTest
    {
        private LoginPage _loginPage = null!;
        private AccountPage _accountPage = null!;

        [SetUp]
        public void CreatePages()
        {
            _loginPage = new LoginPage(Page);
            _accountPage = new AccountPage(Page);
        }

        [Test]
        public async Task ValidCredentials_ReachAccountPage()
        {
            var customer = await CreateRegisteredCustomerAsync();

            await _loginPage.GotoAsync();
            await _loginPage.LoginAsync(customer.Email, customer.Password);

            await Expect(Page).ToHaveURLAsync(UrlEndsWith(AccountPage.Path), new() { Timeout = SlowServerTimeout });
            await Expect(_accountPage.PageTitle).ToHaveTextAsync("My account");
            await Expect(_accountPage.UserMenu).ToContainTextAsync(customer.FullName);
            await Expect(_loginPage.LoginError).ToBeHiddenAsync();
        }

        [Test]
        public async Task WrongPassword_ShowsError_AndStaysOnLogin()
        {
            var customer = await CreateRegisteredCustomerAsync();

            await _loginPage.GotoAsync();
            await _loginPage.LoginAsync(customer.Email, UserFactory.WrongPassword());

            await AssertLoginRejectedAsync();
        }

        [Test]
        public async Task UnregisteredEmail_ShowsError_AndStaysOnLogin()
        {
            await _loginPage.GotoAsync();
            await _loginPage.LoginAsync(UserFactory.UnregisteredEmail(), UserFactory.WrongPassword());

            await AssertLoginRejectedAsync();
        }

        /// <summary>Error is shown and the user is NOT signed in.</summary>
        private async Task AssertLoginRejectedAsync()
        {
            await Expect(_loginPage.LoginError).ToHaveTextAsync("Invalid email or password",
                new() { Timeout = SlowServerTimeout });

            await Expect(Page).ToHaveURLAsync(UrlEndsWith(LoginPage.Path));
            await Expect(_loginPage.NavSignIn).ToBeVisibleAsync();   // still signed out
            await Expect(_accountPage.UserMenu).ToBeHiddenAsync();   // no user menu
        }
    }
}
