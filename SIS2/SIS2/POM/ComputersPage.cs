using Microsoft.Playwright;
using SIS2.POM.Sections;

namespace SIS2.POM
{
    public class ComputersPage
    {
        public readonly IPage _page;
        public ComputersPage(IPage page)
        {
            _page = page;
        }

        public Navbar Navbar => new(_page);

    }
}
