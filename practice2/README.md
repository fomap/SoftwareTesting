# Practice 2 – Toolshop API tests (Playwright for .NET)

Part 2 of the API practice: three requests from the Postman collection
(login → list products → add to cart → read cart) as Playwright API tests.
No browser is opened; the tests use Playwright's `APIRequestContext`.

| Test | Kind | What it checks |
|---|---|---|
| `AddProductToCart_ReturnsOk` | happy path | POST `/carts/{id}` with an in-stock product returns 200 and "item added" |
| `Login_WithUnknownUser_Returns401_AndNoToken` | negative | POST `/users/login` with an unregistered email returns 401, `error: Unauthorized`, no `access_token` |
| `CartReadBack_ContainsTheAddedProduct` | read-back | after adding a product ×2, GET `/carts/{id}` holds exactly that product with quantity 2 |

**Token handling.** As in Postman with `{{token}}`, the token is never pasted into requests:
`ToolshopApiFixture` logs in once per test and creates an `Authed` request context whose
`ExtraHTTPHeaders` carry `Authorization: Bearer <token>` for every call. `BaseUrl` is the `{{baseUrl}}`.
The token lives 300 s, so each test logs in fresh rather than sharing one.

**Credentials.** Defaults to the Postman account (`customer2@practicesoftwaretesting.com` / `welcome01`);
override with the `TOOLSHOP_EMAIL` and `TOOLSHOP_PASSWORD` environment variables.

## Run

```
cd practice2
dotnet test
```
