namespace SIS2.data
{
    public static class Products
    {
        // Buy a product: stock on the shared site changes all the time, so the test buys
        // the first product from this list that is in stock when it runs.
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
        public const string PaymentMethod = "cash-on-delivery"; // needs no extra card/bank fields

        // Compare two products. Stated comparison: Bolt Cutters cost more than Slip Joint Pliers.
        public const string MoreExpensive = "Bolt Cutters";
        public const string Cheaper = "Slip Joint Pliers";
    }
}
