using System.Text.Json;
using Microsoft.Playwright;
using SIS2.data;

namespace SIS2.fixtures
{
    /// <summary>
    /// Test setup through the site's REST API. Creating the account a test needs this
    /// way is fast and keeps the test focused on the flow it is about. Registration
    /// through the UI is covered by RegisterCustomerTests.
    /// </summary>
    public static class AccountApi
    {
        public const string ApiUrl = "https://api.practicesoftwaretesting.com";

        /// <summary>
        /// Returns the customer with street, city and state replaced by what the site's own
        /// postcode lookup says for their country + postcode.
        ///
        /// Why: when an order is placed, the server checks the billing city against this
        /// lookup and rejects anything else ("The city does not belong to the selected
        /// country"), even real cities. The lookup returns generated addresses, so we ask
        /// it at runtime instead of hard-coding one.
        /// </summary>
        public static async Task<CustomerData> WithSiteAddressAsync(IPlaywright playwright, CustomerData c)
        {
            var api = await playwright.APIRequest.NewContextAsync();
            try
            {
                var url = $"{ApiUrl}/postcode-lookup" +
                          $"?country={Uri.EscapeDataString(c.CountryCode)}" +
                          $"&postcode={Uri.EscapeDataString(c.PostalCode)}" +
                          $"&house_number={Uri.EscapeDataString(c.HouseNumber)}";
                var response = await api.GetAsync(url);
                var body = await response.TextAsync();

                Assert.That(response.Ok, Is.True,
                    $"Setup failed: postcode lookup for {c.CountryCode} {c.PostalCode} ({response.Status}): {body}");

                var address = JsonDocument.Parse(body).RootElement;
                return c with
                {
                    Street = address.GetProperty("street").GetString()!,
                    City = address.GetProperty("city").GetString()!,
                    State = address.GetProperty("state").GetString()!,
                };
            }
            finally
            {
                await api.DisposeAsync();
            }
        }

        public static async Task RegisterAsync(IPlaywright playwright, CustomerData c)
        {
            var api = await playwright.APIRequest.NewContextAsync();
            try
            {
                var response = await api.PostAsync(ApiUrl + "/users/register", new()
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
                            country = c.CountryCode,
                            postal_code = c.PostalCode,
                        },
                    },
                });

                // If setup fails, stop here with the server's answer instead of failing later in the UI.
                Assert.That(response.Ok, Is.True,
                    $"Setup failed: could not register {c.Email} via API ({response.Status}): {await response.TextAsync()}");
            }
            finally
            {
                await api.DisposeAsync();
            }
        }
    }
}
