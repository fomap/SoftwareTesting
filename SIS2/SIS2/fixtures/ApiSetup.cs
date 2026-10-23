using System.Text.Json;
using Microsoft.Playwright;
using SIS2.data;

namespace SIS2.fixtures
{
    /// <summary>Test setup through the shop's REST API (fast, and keeps tests about their own flow).</summary>
    public static class ApiSetup
    {
        private const string ApiUrl = "https://api.practicesoftwaretesting.com";

        public static async Task<Customer> RegisterNewCustomerAsync(IPlaywright playwright)
        {
            var c = UserFactory.NewCustomer();
            var api = await playwright.APIRequest.NewContextAsync();
            try
            {
                // At checkout the server only accepts the address its own postcode lookup
                // returns for the postcode (real cities are rejected), so register with that.
                var lookup = await GetJsonAsync(api,
                    $"{ApiUrl}/postcode-lookup?country={c.Country}&postcode={c.PostalCode}&house_number={c.HouseNumber}");
                c = c with
                {
                    Street = lookup.GetProperty("street").GetString()!,
                    City = lookup.GetProperty("city").GetString()!,
                    State = lookup.GetProperty("state").GetString()!,
                };

                var response = await api.PostAsync($"{ApiUrl}/users/register", new()
                {
                    DataObject = new
                    {
                        first_name = c.FirstName,
                        last_name = c.LastName,
                        dob = c.DateOfBirth,
                        email = c.Email,
                        password = c.Password,
                        phone = c.Phone,
                        address = new
                        {
                            street = c.Street,
                            house_number = c.HouseNumber,
                            city = c.City,
                            state = c.State,
                            country = c.Country,
                            postal_code = c.PostalCode,
                        },
                    },
                });
                Assert.That(response.Ok, Is.True, $"Setup: registration failed: {await response.TextAsync()}");
                return c;
            }
            finally
            {
                await api.DisposeAsync();
            }
        }

        /// <summary>
        /// First product from the list that is in stock right now. If none is, the test is
        /// Inconclusive: that is the shared site's state, not a bug in the shop.
        /// </summary>
        public static async Task<string> FirstInStockAsync(IPlaywright playwright, IEnumerable<string> names)
        {
            var api = await playwright.APIRequest.NewContextAsync();
            try
            {
                foreach (var name in names)
                {
                    var found = await GetJsonAsync(api, $"{ApiUrl}/products/search?q={Uri.EscapeDataString(name)}");
                    var inStock = found.GetProperty("data").EnumerateArray().Any(p =>
                        p.GetProperty("name").GetString() == name &&
                        p.GetProperty("in_stock").GetBoolean() &&
                        !p.GetProperty("is_rental").GetBoolean());
                    if (inStock) return name;
                }

                Assert.Inconclusive("None of the candidate products is in stock right now.");
                return null!; // not reached
            }
            finally
            {
                await api.DisposeAsync();
            }
        }

        private static async Task<JsonElement> GetJsonAsync(IAPIRequestContext api, string url)
        {
            var response = await api.GetAsync(url);
            var body = await response.TextAsync();
            Assert.That(response.Ok, Is.True, $"Setup: GET {url} failed: {body}");
            return JsonDocument.Parse(body).RootElement;
        }
    }
}
