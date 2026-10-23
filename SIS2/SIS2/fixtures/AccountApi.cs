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

        public static async Task RegisterAsync(IPlaywright playwright, CustomerData c)
        {
            var api = await playwright.APIRequest.NewContextAsync();
            try
            {
                await PostRegistrationAsync(api, c);
            }
            finally
            {
                await api.DisposeAsync();
            }
        }

        private static async Task PostRegistrationAsync(IAPIRequestContext api, CustomerData c)
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
    }
}
