using Microsoft.Playwright;


namespace SIS1.Debug
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        private IBrowser _browser;

        [Test]
        public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
        {
            await Page.GotoAsync("https://playwright.dev");

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

            // create a locator
            var getStarted = Page.Locator("text=Get Started");

            // Expect an attribute "to be strictly equal" to the value.
            await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

            // Click the get started link.
            await getStarted.ClickAsync();

            // Expects the URL to contain intro.
            await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
        }


        [Test]
        public async Task Test1()
        {
            _browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            var page = await _browser.NewPageAsync();
            //await page.GotoAsync("https://playwright.dev");
            //await Expect(page).ToHaveTitleAsync(new Regex("Playwright"));

            await page.GotoAsync("https://demo.guru99.com/");
            await page.FillAsync("[name='emailid']", "mercury");
            await page.ClickAsync("[name='btnLogin']");

        }

        [Test]
        public async Task Test3()
        {
            var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                Args = new[] {
                            "--disable-blink-features=AutomationControlled"

                }
            });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            await page.GotoAsync("https://www.google.com/");

            var searchBox = page.GetByRole(AriaRole.Combobox, new() { NameRegex = new Regex("Іздеу") });
            await searchBox.FillAsync("Ariana Grande");

            await searchBox.PressAsync("Enter");

            await Expect(page.Locator("#main")).ToContainTextAsync("Ariana Grande");


        }


        [Test]
        public async Task Run()
        {
            using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            await using var browser = await playwright.Webkit.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://www.theverge.com");
            await page.ScreenshotAsync(new() { Path = "theverge.png" });
        }






    }
}
