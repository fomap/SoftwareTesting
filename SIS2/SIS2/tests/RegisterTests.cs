using SIS2.data;
using SIS2.fixtures;

namespace SIS2.tests
{
    /// <summary>Register a new customer: a unique new account is created and the user ends up on the login page.</summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class RegisterTests : PagesFixture
    {
        [Test]
        public async Task NewCustomer_LandsOnLogin_AndAccountExists()
        {
            var customer = UserFactory.NewCustomer(); // unique email every run

            await RegisterPage.OpenAsync();
            await RegisterPage.RegisterAsync(customer);

            await Expect(Page).ToHaveURLAsync(new Regex("/auth/login$"));

            // Proof the account was really created: its credentials work.
            await LoginPage.LoginAsync(customer.Email, customer.Password);
            await Expect(Page).ToHaveURLAsync(new Regex("/account$"));
            await Expect(LoginPage.UserMenu).ToContainTextAsync(customer.FullName);
        }
    }
}
