using SIS2.data;
using SIS2.fixtures;

namespace SIS2.tests
{
    /// <summary>
    /// Compare two products: open two products, capture name and price, assert the stated comparison.
    /// Stated comparison: Bolt Cutters cost more than Slip Joint Pliers.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class CompareProductsTests : PagesFixture
    {
        [Test]
        public async Task BoltCutters_CostMoreThan_SlipJointPliers()
        {
            await ProductPage.SearchAndOpenAsync(Products.MoreExpensive);
            await Expect(ProductPage.Name).ToHaveTextAsync(Products.MoreExpensive); // page loaded
            var first = await ProductPage.ReadNameAndPriceAsync();

            await ProductPage.SearchAndOpenAsync(Products.Cheaper);
            await Expect(ProductPage.Name).ToHaveTextAsync(Products.Cheaper);
            var second = await ProductPage.ReadNameAndPriceAsync();

            Assert.Multiple(() =>
            {
                Assert.That(first.Name, Is.EqualTo(Products.MoreExpensive));
                Assert.That(second.Name, Is.EqualTo(Products.Cheaper));
                Assert.That(first.Price, Is.GreaterThan(second.Price),
                    $"{first.Name} ({first.Price}) should cost more than {second.Name} ({second.Price})");
            });
        }
    }
}
