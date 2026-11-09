using System.Text.Json;
using Microsoft.Playwright;

namespace practice2.fixtures
{
    /// <summary>
    /// The Postman collection, in code. BaseUrl plays the role of {{baseUrl}}, and the token
    /// is set once on the request context (like the {{token}} environment variable),
    /// so no test ever pastes an Authorization header by hand.
    /// </summary>
    public abstract class ToolshopApiFixture : PlaywrightTest
    {
        protected const string BaseUrl = "https://api.practicesoftwaretesting.com";

        // Same account as in the Postman collection; override with environment variables.
        protected static readonly string Email =
            Environment.GetEnvironmentVariable("TOOLSHOP_EMAIL") ?? "customer2@practicesoftwaretesting.com";
        protected static readonly string Password =
            Environment.GetEnvironmentVariable("TOOLSHOP_PASSWORD") ?? "welcome01";

        /// <summary>No token: used for the login call itself.</summary>
        protected IAPIRequestContext Anonymous { get; private set; } = null!;

        /// <summary>Every request sent through this carries "Authorization: Bearer {token}".</summary>
        protected IAPIRequestContext Authed { get; private set; } = null!;

        [SetUp]
        public async Task CreateApiContextsAsync()
        {
            Anonymous = await Playwright.APIRequest.NewContextAsync(new()
            {
                BaseURL = BaseUrl,
                ExtraHTTPHeaders = new Dictionary<string, string> { ["Accept"] = "application/json" },
            });

            var token = await LoginAsync(Email, Password);

            Authed = await Playwright.APIRequest.NewContextAsync(new()
            {
                BaseURL = BaseUrl,
                ExtraHTTPHeaders = new Dictionary<string, string>
                {
                    ["Accept"] = "application/json",
                    ["Authorization"] = $"Bearer {token}",
                },
            });
        }

        [TearDown]
        public async Task DisposeApiContextsAsync()
        {
            if (Authed != null) await Authed.DisposeAsync();
            if (Anonymous != null) await Anonymous.DisposeAsync();
        }

        /// <summary>POST /users/login and return the access_token (fails the test if login fails).</summary>
        protected async Task<string> LoginAsync(string email, string password)
        {
            var response = await Anonymous.PostAsync("/users/login", new()
            {
                DataObject = new { email, password },
            });
            var body = await ReadJsonAsync(response);
            Assert.That(response.Status, Is.EqualTo(200), $"Setup: login failed: {body}");
            return body.GetProperty("access_token").GetString()!;
        }

        /// <summary>GET /products and return the first product that can be put in a cart.</summary>
        protected async Task<JsonElement> FirstBuyableProductAsync()
        {
            var response = await Authed.GetAsync("/products");
            Assert.That(response.Status, Is.EqualTo(200), "Setup: GET /products failed");
            var products = (await ReadJsonAsync(response)).GetProperty("data").EnumerateArray();

            foreach (var p in products)
                if (p.GetProperty("in_stock").GetBoolean() && !p.GetProperty("is_rental").GetBoolean())
                    return p;

            Assert.Inconclusive("No in-stock, non-rental product on the first page of /products.");
            return default; // not reached
        }

        /// <summary>POST /carts and return the new cart id.</summary>
        protected async Task<string> CreateCartAsync()
        {
            var response = await Authed.PostAsync("/carts");
            var body = await ReadJsonAsync(response);
            Assert.That(response.Status, Is.EqualTo(201), $"Setup: POST /carts failed: {body}");
            return body.GetProperty("id").GetString()!;
        }

        /// <summary>POST /carts/{id} with a product and quantity.</summary>
        protected Task<IAPIResponse> AddToCartAsync(string cartId, string productId, int quantity) =>
            Authed.PostAsync($"/carts/{cartId}", new()
            {
                DataObject = new { product_id = productId, quantity },
            });

        protected static async Task<JsonElement> ReadJsonAsync(IAPIResponse response)
        {
            var text = await response.TextAsync();
            return JsonDocument.Parse(string.IsNullOrWhiteSpace(text) ? "{}" : text).RootElement.Clone();
        }
    }
}
