using System.Text.Json;
using practice2.fixtures;

namespace practice2.tests
{
    /// <summary>
    /// Part 2: three requests from the Postman collection as Playwright API tests.
    /// One happy path, one negative case, one that reads back what it created.
    /// </summary>
    [TestFixture]
    public class CartApiTests : ToolshopApiFixture
    {
        // 1. Happy path: logged in, list products, add one to a new cart -> accepted.
        [Test]
        public async Task AddProductToCart_ReturnsOk()
        {
            var product = await FirstBuyableProductAsync();
            var cartId = await CreateCartAsync();

            var response = await AddToCartAsync(cartId, product.GetProperty("id").GetString()!, 1);
            var body = await ReadJsonAsync(response);

            Assert.That(response.Status, Is.EqualTo(200), body.ToString());
            Assert.That(body.GetProperty("result").GetString(), Does.Contain("added"));
        }

        // 2. Negative: login with an email that is not registered -> 401 and no token.
        //    (An unregistered email, not a wrong password for customer2: repeated failed
        //    logins lock the shared seeded account for everyone.)
        [Test]
        public async Task Login_WithUnknownUser_Returns401_AndNoToken()
        {
            var unknownEmail = $"nobody.{Guid.NewGuid():N}@example.com";

            var response = await Anonymous.PostAsync("/users/login", new()
            {
                DataObject = new { email = unknownEmail, password = "welcome01" },
            });
            var body = await ReadJsonAsync(response);

            Assert.That(response.Status, Is.EqualTo(401), body.ToString());
            Assert.That(body.TryGetProperty("access_token", out _), Is.False, "No token may be issued");
            Assert.That(body.GetProperty("error").GetString(), Is.EqualTo("Unauthorized"));
        }

        // 3. Read back: add a product with quantity 2, then GET /carts/{id} shows exactly that item.
        [Test]
        public async Task CartReadBack_ContainsTheAddedProduct()
        {
            var product = await FirstBuyableProductAsync();
            var productId = product.GetProperty("id").GetString()!;
            const int quantity = 2;

            var cartId = await CreateCartAsync();
            var add = await AddToCartAsync(cartId, productId, quantity);
            Assert.That(add.Status, Is.EqualTo(200), $"Add to cart failed: {await add.TextAsync()}");

            var response = await Authed.GetAsync($"/carts/{cartId}");
            var cart = await ReadJsonAsync(response);

            Assert.That(response.Status, Is.EqualTo(200), cart.ToString());
            Assert.That(cart.GetProperty("id").GetString(), Is.EqualTo(cartId).IgnoreCase);

            var items = cart.GetProperty("cart_items").EnumerateArray().ToList();
            Assert.That(items, Has.Count.EqualTo(1), "A new cart with one add should hold one line");

            var item = items[0];
            // The API returns cart ids lower-case and product ids upper-case, so compare ignoring case.
            Assert.That(item.GetProperty("product_id").GetString(), Is.EqualTo(productId).IgnoreCase);
            Assert.That(item.GetProperty("quantity").GetInt32(), Is.EqualTo(quantity));
            Assert.That(item.GetProperty("product").GetProperty("name").GetString(),
                Is.EqualTo(product.GetProperty("name").GetString()));
        }
    }
}
