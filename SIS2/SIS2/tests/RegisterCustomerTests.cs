using Microsoft.Playwright;
using SIS2.data;
using SIS2.fixtures;
using SIS2.pages;

namespace SIS2.tests
{
    /// <summary>
    /// Test case 1: Register a new customer.
    /// Must prove: a unique new account is created and the user ends up signed in
    /// or on the login page.
    ///
    /// Parallel/order safety:
    ///  - every test creates its own fresh customer via UserFactory (no shared data);
    ///  - PageTest gives every test its own browser context (no shared cookies/login);
    ///  - InstancePerTestCase gives every test its own fixture object (no shared fields);
    ///  - assertions only look at this test's own user, never at counts or lists
    ///    that other people on the public site could change.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class RegisterCustomerTests : BaseTest
    {
        private RegisterPage _registerPage = null!;
        private LoginPage _loginPage = null!;
        private AccountPage _accountPage = null!;

        [SetUp]
        public void CreatePages()
        {
            _registerPage = new RegisterPage(Page);
            _loginPage = new LoginPage(Page);
            _accountPage = new AccountPage(Page);
        }

        [Test]
        public async Task RegisterNewCustomer_LandsOnLogin_AndNewAccountCanSignIn()
        {
            var customer = UserFactory.NewCustomer();

            await _registerPage.GotoAsync();
            await _registerPage.RegisterAsync(customer);

            // 1. The user ends up on the login page.
            await Expect(Page).ToHaveURLAsync(UrlEndsWith(LoginPage.Path), new() { Timeout = SlowServerTimeout });
            await Expect(_loginPage.SubmitButton).ToBeVisibleAsync();

            // 2. The account really exists: its credentials work and open *this* user's account.
            await _loginPage.LoginAsync(customer.Email, customer.Password);

            await Expect(Page).ToHaveURLAsync(UrlEndsWith(AccountPage.Path), new() { Timeout = SlowServerTimeout });
            await Expect(_accountPage.PageTitle).ToHaveTextAsync("My account");
            await Expect(_accountPage.UserMenu).ToContainTextAsync(customer.FullName);
        }

        [Test]
        public async Task RegisterSameEmailTwice_SecondAttemptIsRejected()
        {
            var customer = UserFactory.NewCustomer();

            // First registration succeeds.
            await _registerPage.GotoAsync();
            await _registerPage.RegisterAsync(customer);
            await Expect(Page).ToHaveURLAsync(UrlEndsWith(LoginPage.Path), new() { Timeout = SlowServerTimeout });

            // Second registration with the same email must not create another account.
            await _registerPage.GotoAsync();
            await _registerPage.RegisterAsync(customer);

            await Expect(_registerPage.RegisterError).ToContainTextAsync("already exists", new() { Timeout = SlowServerTimeout });
            await Expect(Page).ToHaveURLAsync(UrlEndsWith(RegisterPage.Path));
        }
    }
}
