namespace SIS2.data
{
    /// <summary>
    /// Products the tests work with. They come from the site's standard catalogue.
    /// Tests find them by searching for the name, never by product id,
    /// because ids are regenerated when the demo database is reset.
    /// </summary>
    public static class Products
    {
        // Buy a product.
        // Stock on the shared site goes up and down as people (and our own test runs)
        // buy things, so no single product is guaranteed to be in stock. The test buys
        // the first product from this list that is in stock at the moment it runs.
        public static readonly string[] BuyCandidates =
        {
            "Adjustable Wrench",
            "Angled Spanner",
            "Belt Sander",
            "Bolt Cutters",
            "Chisels Set",
            "Circular Saw",
            "Claw Hammer",
            "Claw Hammer with Fiberglass Handle",
            "Claw Hammer with Shock Reduction Grip",
            "Combination Pliers",
            "Construction Helmet",
            "Cordless Drill 12V",
            "Cordless Drill 24V",
            "Court Hammer",
            "Cross-head screws",
            "Drawer Tool Cabinet",
            "Ear Protection",
            "Flat-Head Wood Screws",
            "Hammer",
            "Leather toolbelt",
            "Long Nose Pliers",
            "M4 Nuts",
            "Measuring Tape",
            "Mini Screwdriver",
            "Nuts and bolts",
            "Open-end Spanners (Set)",
            "Phillips Screwdriver",
            "Pliers",
            "Protective Gloves",
            "Safety Goggles",
            "Safety Helmet Face Shield",
            "Screws",
            "Sheet Sander",
            "Sledgehammer",
            "Slip Joint Pliers",
            "Square Ruler",
            "Super-thin Protection Gloves",
            "Swiss Woodcarving Chisels",
            "Tape Measure 5m",
            "Tape Measure 7.5m",
            "Thor Hammer",
            "Tool Cabinet",
            "Washers",
            "Wood Carving Chisels",
            "Wood Saw",
        };
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
