using Microsoft.Playwright;
using SIS2.POM;



namespace SIS2
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class LoginPOM : PageTest
    {
        private LoginPage _loginPage;
        private ComputersPage _computersPage;

        [SetUp]
        public void SetUp()
        {
            _loginPage = new LoginPage(Page);
            _computersPage = new ComputersPage(Page);
        }

        [Test]
        public async Task LoginTest()
        {
            await _loginPage.GoTo();
            await _loginPage.Login("test", "test");
            await Expect(_computersPage.Navbar.Logout).ToBeVisibleAsync();

        }

    }

}