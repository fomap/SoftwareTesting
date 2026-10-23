using Microsoft.Playwright;

namespace SIS2.POM.Sections
{
    public class Navbar
    {
        public readonly IPage _page;
        public Navbar(IPage page)
        {
            _page = page;
        }


        public ILocator Logout => _page.GetByText("Logout");

    }
}
