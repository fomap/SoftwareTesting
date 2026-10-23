using Microsoft.Playwright;
using SIS2.data;
using SIS2.fixtures;
using SIS2.pages;

namespace SIS2.tests
{
    /// <summary>
    /// Test case 4: Compare two products.
    /// Must prove: open two products, capture name and price, assert the comparison you state.
    ///
    /// Stated comparison: "Bolt Cutters cost more than Slip Joint Pliers"
    /// (names and statement live in data/Products.cs).
    /// The test is read-only (no account, no cart), so it is safe to run in parallel
    /// and alongside anyone else using the site.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class CompareProductsTests : BaseTest
    {
        private HomePage _homePage = null!;
        private ProductPage _productPage = null!;

        [SetUp]
        public void CreatePages()
        {
            _homePage = new HomePage(Page);
            _productPage = new ProductPage(Page);
        }

        [Test]
        public async Task MoreExpensiveProduct_CostsMoreThan_CheaperProduct()
        {
            var expensive = await OpenAndCaptureAsync(Products.MoreExpensive);
            var cheaper = await OpenAndCaptureAsync(Products.Cheaper);

            NUnit.Framework.TestContext.WriteLine($"Captured: {expensive.Name} = {Prices.Format(expensive.Price)}, {cheaper.Name} = {Prices.Format(cheaper.Price)}");

            Assert.Multiple(() =>
            {
                // We really opened the two products we meant to compare.
                Assert.That(expensive.Name, Is.EqualTo(Products.MoreExpensive));
                Assert.That(cheaper.Name, Is.EqualTo(Products.Cheaper));
                Assert.That(expensive.Name, Is.Not.EqualTo(cheaper.Name));

                // Both prices were read as real amounts.
                Assert.That(expensive.Price, Is.GreaterThan(0m));
                Assert.That(cheaper.Price, Is.GreaterThan(0m));

                // The stated comparison.
                Assert.That(expensive.Price, Is.GreaterThan(cheaper.Price),
                    $"Expected {expensive.Name} ({expensive.Price}) to cost more than {cheaper.Name} ({cheaper.Price})");
            });
        }

        /// <summary>
        /// Finds the product by search, notes the price on its listing card, opens it,
        /// checks the product page shows that product at the same price, and returns
        /// the name and price captured from the product page.
        /// </summary>
        private async Task<ProductSnapshot> OpenAndCaptureAsync(string name)
        {
            await _homePage.GotoAsync();
            await _homePage.SearchAsync(name);
            var listingPrice = await _homePage.GetCardPriceAsync(name);

            await _homePage.OpenProductAsync(name);
            await Expect(_productPage.Name).ToHaveTextAsync(name);

            var captured = await _productPage.CaptureAsync();
            Assert.That(captured.Price, Is.EqualTo(listingPrice),
                $"{name}: product page price differs from the listing price");
            return captured;
        }
    }
}
