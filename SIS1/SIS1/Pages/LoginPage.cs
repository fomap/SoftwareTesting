using Microsoft.Playwright;


namespace SIS1.Pages
{
    public class LoginPage()
    {
        private IPage _page;
        private readonly ILocator _lnkLogin;
        private readonly ILocator _txtUserName;
        private readonly ILocator _txtPassword;
        private readonly ILocator _btnLogin;
        private readonly ILocator _lnkEmployeeDetails;


        public LoginPage(IPage page) : this(Page)
        {
            _page = page;
            _lnkLogin = _page.Locator("text=Login");
            _txtUserName = _page.Locator("#UserName");
            _txtPassword = _page.Locator("#Password");
            _btnLogin = _page.Locator("text=Log in");
            _lnkEmployeeDetails = _page.Locator("text='Employee Details'");

        }



    }



}