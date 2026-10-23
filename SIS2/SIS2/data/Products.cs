namespace SIS2.data
{
    /// <summary>
    /// Products the tests work with. They come from the site's standard catalogue,
    /// are in stock, and have names that match exactly one product.
    /// Tests find them by searching for the name, never by product id,
    /// because ids are regenerated when the demo database is reset.
    /// </summary>
    public static class Products
    {
        // Buy a product
        public const string ToBuy = "Combination Pliers";
        public const int QuantityToBuy = 2;

        // Compare two products.
        // The comparison we state: Bolt Cutters cost more than Slip Joint Pliers.
        public const string MoreExpensive = "Bolt Cutters";
        public const string Cheaper = "Slip Joint Pliers";
    }

    /// <summary>Payment methods offered in the checkout's last step (option values).</summary>
    public static class PaymentMethods
    {
        // Needs no extra card/bank fields, so the test stays about the purchase itself.
        public const string CashOnDelivery = "cash-on-delivery";
    }

    /// <summary>Name and price of a product exactly as the page showed them.</summary>
    public sealed record ProductSnapshot(string Name, decimal Price);
}
