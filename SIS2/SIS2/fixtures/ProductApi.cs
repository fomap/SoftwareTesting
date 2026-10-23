using System.Text.Json;
using Microsoft.Playwright;

namespace SIS2.fixtures
{
    /// <summary>Read-only product lookups through the site's REST API, used for test setup.</summary>
    public static class ProductApi
    {
        /// <summary>
        /// Returns the first product name from <paramref name="candidates"/> that is in stock
        /// right now (and is a normal product, not a rental). If none is, the test is marked
        /// Inconclusive: the precondition "something can be bought" is not met, which is a
        /// state of the shared site, not a bug in the shop.
        /// </summary>
        public static async Task<string> FirstInStockAsync(IPlaywright playwright, IEnumerable<string> candidates)
        {
            var api = await playwright.APIRequest.NewContextAsync();
            try
            {
                var outOfStock = new List<string>();

                foreach (var name in candidates)
                {
                    var response = await api.GetAsync(
                        $"{AccountApi.ApiUrl}/products/search?q={Uri.EscapeDataString(name)}");
                    var body = await response.TextAsync();
                    Assert.That(response.Ok, Is.True,
                        $"Setup failed: product search for '{name}' ({response.Status}): {body}");

                    // Search matches parts of names ("Pliers" also finds "Slip Joint Pliers"),
                    // so pick the product whose name is exactly the one we asked for.
                    foreach (var product in JsonDocument.Parse(body).RootElement.GetProperty("data").EnumerateArray())
                    {
                        if (product.GetProperty("name").GetString() != name) continue;

                        var inStock = product.GetProperty("in_stock").GetBoolean();
                        var isRental = product.GetProperty("is_rental").GetBoolean();
                        if (inStock && !isRental) return name;

                        outOfStock.Add(name);
                    }
                }

                Assert.Inconclusive(
                    "None of the candidate products is in stock right now: " + string.Join(", ", outOfStock));
                return null!; // not reached: Inconclusive throws
            }
            finally
            {
                await api.DisposeAsync();
            }
        }
    }
}
